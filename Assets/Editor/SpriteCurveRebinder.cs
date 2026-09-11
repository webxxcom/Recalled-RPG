using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Rebinds object-reference sprite curves in an AnimationClip from
/// SpriteRenderer to UI.Image.
///
/// The Aseprite importer generates clips that key SpriteRenderer.m_Sprite.
/// A UI object has an Image, not a SpriteRenderer, so the curve binds to
/// nothing and the animation silently does nothing. Both components serialize
/// the sprite under the same property name (m_Sprite), so only the binding's
/// declared type needs to change — the keyframes carry over untouched.
///
/// Usage: select one or more AnimationClip assets in the Project window,
/// right-click, "Rebind Sprite Curves to UI Image".
///
/// NOTE: importer-generated clips are sub-assets and read-only. Export the
/// clips as standalone assets first (Aseprite import settings → export
/// Animation Clips), then run this on the exported copies.
/// </summary>
public static class SpriteCurveRebinder
{
    private const string SpriteProperty = "m_Sprite";
    private const string MenuPath = "Assets/Rebind Sprite Curves to UI Image";

    [MenuItem(MenuPath, true)]
    private static bool ValidateSelection()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is AnimationClip)
                return true;
        }

        return false;
    }

    [MenuItem(MenuPath)]
    private static void RebindSelection()
    {
        int rebound = 0;
        int skipped = 0;

        foreach (Object obj in Selection.objects)
        {
            if (obj is not AnimationClip clip)
                continue;

            if (IsReadOnly(clip))
            {
                Debug.LogWarning(
                    $"'{clip.name}' is read-only (an importer sub-asset). " +
                    "Export the clip as a standalone asset first.", clip);
                skipped++;
                continue;
            }

            if (Rebind(clip))
                rebound++;
        }

        if (rebound > 0)
            AssetDatabase.SaveAssets();

        Debug.Log($"Rebound {rebound} clip(s) to UI.Image. Skipped {skipped}.");
    }

    /// <summary>
    /// Moves every SpriteRenderer.m_Sprite curve in the clip onto Image.m_Sprite.
    /// Returns true if anything changed.
    /// </summary>
    public static bool Rebind(AnimationClip clip)
    {
        if (clip == null)
            return false;

        // Snapshot: the array is a copy, so mutating curves while iterating is safe.
        EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

        bool changed = false;

        foreach (EditorCurveBinding binding in bindings)
        {
            if (binding.type != typeof(SpriteRenderer))
                continue;

            if (binding.propertyName != SpriteProperty)
                continue;

            ObjectReferenceKeyframe[] keyframes =
                AnimationUtility.GetObjectReferenceCurve(clip, binding);

            if (keyframes == null || keyframes.Length == 0)
                continue;

            if (!changed)
                Undo.RecordObject(clip, "Rebind Sprite Curves to UI Image");

            // Passing null removes the old curve.
            AnimationUtility.SetObjectReferenceCurve(clip, binding, null);

            EditorCurveBinding uiBinding = EditorCurveBinding.PPtrCurve(
                binding.path,
                typeof(Image),
                SpriteProperty);

            AnimationUtility.SetObjectReferenceCurve(clip, uiBinding, keyframes);

            changed = true;
        }

        if (changed)
            EditorUtility.SetDirty(clip);

        return changed;
    }

    private static bool IsReadOnly(AnimationClip clip)
    {
        return (clip.hideFlags & HideFlags.NotEditable) != 0;
    }
}
