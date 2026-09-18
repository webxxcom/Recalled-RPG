using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Inspector for SpriteSequence: pick a source .aseprite, pick one of its
/// clips, extract.
///
/// The source is stored as a GUID on the asset, but drawn here as an
/// ObjectField so it behaves like a normal drag-and-drop reference.
/// </summary>
[CustomEditor(typeof(SpriteSequence))]
public class SpriteSequenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var sequence = (SpriteSequence)target;

        serializedObject.Update();

        DrawSource(sequence);
        EditorGUILayout.Space();

        DrawDefaultInspector();
        EditorGUILayout.Space();

        DrawExtractButton(sequence);
        DrawSummary(sequence);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSource(SpriteSequence sequence)
    {
        EditorGUILayout.LabelField("Source", EditorStyles.boldLabel);

        string path = AssetDatabase.GUIDToAssetPath(sequence.SourceGuid);
        var current = string.IsNullOrEmpty(path)
            ? null
            : AssetDatabase.LoadAssetAtPath<Object>(path);

        EditorGUI.BeginChangeCheck();
        var picked = EditorGUILayout.ObjectField("Aseprite File", current, typeof(Object), false);

        if (EditorGUI.EndChangeCheck())
            AssignSource(sequence, picked);

        if (string.IsNullOrEmpty(path))
        {
            EditorGUILayout.HelpBox("Assign the .aseprite file to extract from.", MessageType.Info);
            return;
        }

        DrawClipPicker(sequence, path);
    }

    private static void AssignSource(SpriteSequence sequence, Object picked)
    {
        Undo.RecordObject(sequence, "TrySet Sprite Sequence Source");

        if (picked == null)
        {
            sequence.SourceGuid = null;
            sequence.ClipName = null;
        }
        else
        {
            string pickedPath = AssetDatabase.GetAssetPath(picked);

            if (!SpriteSequenceExtractor.IsAsepritePath(pickedPath))
            {
                Debug.LogWarning("Source must be an .aseprite or .ase file.");
                return;
            }

            sequence.SourceGuid = AssetDatabase.AssetPathToGUID(pickedPath);
            sequence.ClipName = null;
        }

        EditorUtility.SetDirty(sequence);
    }

    private static void DrawClipPicker(SpriteSequence sequence, string path)
    {
        string[] clipNames = SpriteSequenceExtractor.GetClipNames(path);

        if (clipNames.Length == 0)
        {
            EditorGUILayout.HelpBox(
                "No clips in this file. TrySet its import mode to Animated Sprite, " +
                "and check it has more than one frame.",
                MessageType.Warning);
            return;
        }

        int index = System.Array.IndexOf(clipNames, sequence.ClipName);

        // Default to the first clip rather than showing an empty picker.
        if (index < 0)
            index = 0;

        EditorGUI.BeginChangeCheck();
        index = EditorGUILayout.Popup("Clip", index, clipNames);

        if (EditorGUI.EndChangeCheck() || sequence.ClipName != clipNames[index])
        {
            Undo.RecordObject(sequence, "TrySet Sprite Sequence Clip");
            sequence.ClipName = clipNames[index];
            EditorUtility.SetDirty(sequence);
        }
    }

    private static void DrawExtractButton(SpriteSequence sequence)
    {
        using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(sequence.SourceGuid)))
        {
            if (GUILayout.Button("Extract Frames", GUILayout.Height(24)))
                Extract(sequence);
        }
    }

    private static void Extract(SpriteSequence sequence)
    {
        string path = AssetDatabase.GUIDToAssetPath(sequence.SourceGuid);

        if (!SpriteSequenceExtractor.TryExtract(
                path,
                sequence.ClipName,
                out List<SpriteSequence.Frame> frames,
                out float length,
                out string error))
        {
            Debug.LogError($"{sequence.name}: {error}", sequence);
            return;
        }

        Undo.RecordObject(sequence, "Extract Sprite Sequence Frames");
        sequence.SetGeneratedData(frames, length);
        EditorUtility.SetDirty(sequence);
        AssetDatabase.SaveAssets();
    }

    private static void DrawSummary(SpriteSequence sequence)
    {
        if (!sequence.IsValid)
        {
            EditorGUILayout.HelpBox("No frames extracted yet.", MessageType.Info);
            return;
        }

        EditorGUILayout.HelpBox(
            $"{sequence.Frames.Count} frames, {sequence.Length:F3}s" +
            (sequence.Loop ? ", looping" : ""),
            MessageType.None);
    }
}
