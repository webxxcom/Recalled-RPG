using UnityEngine;

namespace Recalled.UI
{
    public static class TransformExtension
    {
        public static Rect GetRectInSpace(this RectTransform target, Transform space, Vector3[] s_corners)
        {
            target.GetWorldCorners(s_corners);

            Vector2 min = new(float.PositiveInfinity, float.PositiveInfinity);
            Vector2 max = new(float.NegativeInfinity, float.NegativeInfinity);

            for (int i = 0; i < s_corners.Length; i++)
            {
                Vector2 local = space.InverseTransformPoint(s_corners[i]);
                min = Vector2.Min(min, local);
                max = Vector2.Max(max, local);
            }

            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }
    }
}