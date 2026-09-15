using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Pulls sprite keyframes out of an Aseprite-generated AnimationClip.
///
/// Shared by the SpriteSequence inspector button and the refresh
/// postprocessor, so manual extraction and automatic refresh can't diverge.
/// </summary>
public static class SpriteSequenceExtractor
{
    private const string SpriteProperty = "m_Sprite";

    public static bool IsAsepritePath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        string extension = Path.GetExtension(path).ToLowerInvariant();
        return extension == ".aseprite" || extension == ".ase";
    }

    /// <summary>
    /// Names of every clip the importer generated for this file. These are the
    /// Aseprite tag names, or "&lt;name&gt;_Clip" when the file has no tags.
    /// </summary>
    public static string[] GetClipNames(string asepritePath)
    {
        var names = new List<string>();

        foreach (AnimationClip clip in LoadClips(asepritePath))
            names.Add(clip.name);

        return names.ToArray();
    }

    /// <summary>
    /// Extracts frames for one clip. Returns false with a reason in
    /// <paramref name="error"/> rather than throwing, so callers can report it.
    /// </summary>
    public static bool TryExtract(
        string asepritePath,
        string clipName,
        out List<SpriteSequence.Frame> frames,
        out float length,
        out string error)
    {
        frames = null;
        length = 0f;
        error = null;

        if (!IsAsepritePath(asepritePath))
        {
            error = "Source is not an .aseprite file.";
            return false;
        }

        List<AnimationClip> clips = LoadClips(asepritePath);

        if (clips.Count == 0)
        {
            // Not necessarily a mistake: the importer skips clip generation for
            // single-frame files and for import modes other than AnimatedSprite.
            error = "No animation clips in this file. Check the import mode is " +
                    "Animated Sprite and the file has more than one frame.";
            return false;
        }

        AnimationClip target = null;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
            {
                target = clip;
                break;
            }
        }

        if (target == null)
        {
            error = $"No clip named '{clipName}'. Available: {string.Join(", ", GetClipNames(asepritePath))}";
            return false;
        }

        if (!TryReadSpriteCurve(target, out ObjectReferenceKeyframe[] keyframes, out error))
            return false;

        frames = new List<SpriteSequence.Frame>(keyframes.Length);

        foreach (ObjectReferenceKeyframe keyframe in keyframes)
        {
            frames.Add(new SpriteSequence.Frame
            {
                Time = keyframe.time,
                Sprite = keyframe.value as Sprite
            });
        }

        length = target.length;
        return true;
    }

    private static List<AnimationClip> LoadClips(string asepritePath)
    {
        var clips = new List<AnimationClip>();

        if (!IsAsepritePath(asepritePath))
            return clips;

        foreach (Object asset in AssetDatabase.LoadAllAssetsAtPath(asepritePath))
        {
            if (asset is AnimationClip clip)
                clips.Add(clip);
        }

        return clips;
    }

    private static bool TryReadSpriteCurve(
        AnimationClip clip,
        out ObjectReferenceKeyframe[] keyframes,
        out string error)
    {
        keyframes = null;
        error = null;

        EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

        EditorCurveBinding? spriteBinding = null;
        int count = 0;

        foreach (EditorCurveBinding binding in bindings)
        {
            if (binding.propertyName != SpriteProperty)
                continue;

            count++;

            if (spriteBinding == null)
                spriteBinding = binding;
        }

        if (spriteBinding == null)
        {
            error = $"Clip '{clip.name}' has no sprite curve.";
            return false;
        }

        if (count > 1)
        {
            // Happens when the import mode splits Aseprite layers into separate
            // GameObjects, giving one curve per layer. A single Image can only
            // show one sprite, so the rest are dropped.
            Debug.LogWarning(
                $"Clip '{clip.name}' animates {count} sprite curves. " +
                $"Using '{spriteBinding.Value.path}' and ignoring the rest.");
        }

        keyframes = AnimationUtility.GetObjectReferenceCurve(clip, spriteBinding.Value);

        if (keyframes == null || keyframes.Length == 0)
        {
            error = $"Clip '{clip.name}' has an empty sprite curve.";
            return false;
        }

        return true;
    }
}
