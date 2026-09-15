using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Re-extracts existing SpriteSequence assets when their source .aseprite is
/// reimported.
///
/// This is the half the manual button can't cover: creating a sequence is
/// explicit (you make the asset), but keeping it current shouldn't be. Retime
/// an animation in Aseprite and every sequence pointing at that file follows,
/// with no button to remember.
///
/// It never creates sequences — only refreshes ones that already exist.
/// </summary>
public class SpriteSequenceRefreshPostprocessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        var guids = new HashSet<string>();

        foreach (string path in importedAssets)
        {
            if (!SpriteSequenceExtractor.IsAsepritePath(path))
                continue;

            string guid = AssetDatabase.AssetPathToGUID(path);

            if (!string.IsNullOrEmpty(guid))
                guids.Add(guid);
        }

        if (guids.Count == 0)
            return;

        // Deferred: writing assets from inside the import callback invites
        // AssetDatabase reentrancy trouble. Also lets the .aseprite finish
        // importing before we read its generated clips.
        //
        // No loop guard needed — we only react to .aseprite imports, and the
        // .asset files we touch never match that filter.
        EditorApplication.delayCall += () => RefreshSequencesFor(guids);
    }

    private static void RefreshSequencesFor(HashSet<string> sourceGuids)
    {
        // Scoped to sequence assets and only runs on .aseprite imports, so the
        // project-wide search is cheap in practice.
        string[] sequenceGuids = AssetDatabase.FindAssets($"t:{nameof(SpriteSequence)}");

        int refreshed = 0;

        foreach (string sequenceGuid in sequenceGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(sequenceGuid);
            var sequence = AssetDatabase.LoadAssetAtPath<SpriteSequence>(path);

            if (sequence == null || string.IsNullOrEmpty(sequence.SourceGuid))
                continue;

            if (!sourceGuids.Contains(sequence.SourceGuid))
                continue;

            if (Refresh(sequence))
                refreshed++;
        }

        if (refreshed > 0)
            AssetDatabase.SaveAssets();
    }

    private static bool Refresh(SpriteSequence sequence)
    {
        string sourcePath = AssetDatabase.GUIDToAssetPath(sequence.SourceGuid);

        if (!SpriteSequenceExtractor.TryExtract(
                sourcePath,
                sequence.ClipName,
                out List<SpriteSequence.Frame> frames,
                out float length,
                out string error))
        {
            // A renamed or deleted Aseprite tag lands here. Warn rather than
            // clear the data: stale frames still play, empty ones don't, and
            // the warning tells you which asset needs attention.
            Debug.LogWarning($"{sequence.name}: {error}", sequence);
            return false;
        }

        sequence.SetGeneratedData(frames, length);
        EditorUtility.SetDirty(sequence);
        return true;
    }
}
