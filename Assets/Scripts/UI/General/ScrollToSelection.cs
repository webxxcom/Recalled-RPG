using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Recalled.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect))]
    public sealed class ScrollToSelection : MonoBehaviour
    {
        [SerializeField] bool _followSelection = true;

        [Tooltip("Space kept between the target and the viewport edge, in viewport units. " +
                 "Make it at least as large as the highlighter frame's overhang past the item.")]
        [SerializeField] Vector2 _padding = new(8f, 8f);

        [SerializeField] bool _smooth = true;

        [Tooltip("How quickly the smooth scroll converges. Higher is snappier.")]
        [SerializeField, Min(0.01f)] float _sharpness = 15f;

        const float SnapEpsilon = 0.0005f;

        // Shared buffer for GetWorldCorners, so measuring never allocates.
        // Safe because UI code runs on the main thread only.
        static readonly Vector3[] s_corners = new Vector3[4];

        ScrollRect _scrollRect;
        GameObject _lastSelected;
        bool _hasTrackedSinceEnable;
        bool _isAnimating;
        Vector2 _targetNormalized;
        Vector2 _lastAppliedNormalized;
        RectTransform Viewport => _scrollRect.viewport != null
            ? _scrollRect.viewport
            : (RectTransform)_scrollRect.transform;

        void Awake() => _scrollRect = GetComponent<ScrollRect>();

        void OnEnable()
        {
            _lastSelected = null;
            _hasTrackedSinceEnable = false;
            _isAnimating = false;
        }

        void LateUpdate()
        {
            // LateUpdate: the EventSystem processes input in Update, so by now
            // this frame's selection change has already happened.
            if (_followSelection)
                TrackSelection();

            if (_isAnimating)
                StepAnimation();
        }

        /// <summary>
        /// Scrolls so that <paramref name="target"/> is fully visible.
        /// Call this directly when something other than EventSystem selection decides
        /// what must be visible, for example a highlighter's destination rectangle.
        /// </summary>
        /// <param name="target">A RectTransform inside this ScrollRect's content.</param>
        /// <param name="instant">Snap immediately, ignoring the smooth setting.</param>
        public void EnsureVisible(RectTransform target, bool instant = false)
        {
            if (target == null || _scrollRect.content == null)
                return;

            if (!target.IsChildOf(_scrollRect.content))
            {
                Debug.LogWarning($"{name}: '{target.name}' is not inside the scroll content.", this);
                return;
            }

            // Layout groups and content size fitters rebuild later in the frame.
            // Rebuild now so the positions measured below are current.
            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.content);

            // Leftover inertia would fight the correction.
            _scrollRect.StopMovement();

            // Measure everything in the viewport's local space, the same space
            // ScrollRect itself uses to compute its normalized position.
            RectTransform viewport = Viewport;
            Rect view = viewport.rect;
            Rect item = target.GetRectInSpace(viewport, s_corners);
            Rect content = _scrollRect.content.GetRectInSpace(viewport, s_corners);

            Vector2 current = _scrollRect.normalizedPosition;
            Vector2 next = current;

            if (_scrollRect.horizontal)
            {
                // Rows read left to right: if the item is wider than the viewport, keep its left edge visible.
                float delta = ComputeDelta(item.xMin, item.xMax, view.xMin, view.xMax, _padding.x, keepMaxEdge: false);
                next.x = ApplyDelta(current.x, delta, content.width - view.width);
            }

            if (_scrollRect.vertical)
            {
                // Lists read top-down: if the item is taller than the viewport, keep its top edge visible.
                float delta = ComputeDelta(item.yMin, item.yMax, view.yMin, view.yMax, _padding.y, keepMaxEdge: true);
                next.y = ApplyDelta(current.y, delta, content.height - view.height);
            }

            if (instant || !_smooth)
            {
                _isAnimating = false;
                Apply(next);
                return;
            }

            _targetNormalized = next;
            _lastAppliedNormalized = current;
            _isAnimating = true;
        }

        void TrackSelection()
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            // Comparing one reference per frame is negligible; the expensive work
            // (layout rebuild and measuring) only runs when the selection changes.
            GameObject selected = eventSystem.currentSelectedGameObject;
            if (selected == _lastSelected && _hasTrackedSinceEnable)
                return;

            bool isFirstSinceEnable = !_hasTrackedSinceEnable;
            _hasTrackedSinceEnable = true;
            _lastSelected = selected;

            if (selected == null || selected.transform is not RectTransform target)
                return;

            // A selection elsewhere in the UI is normal, not an error: ignore it quietly.
            if (_scrollRect.content == null || !target.IsChildOf(_scrollRect.content))
                return;

            // Snap on the first selection after opening, so the list doesn't visibly scroll as the panel appears.
            EnsureVisible(target, instant: isFirstSinceEnable);
        }

        void StepAnimation()
        {
            Vector2 current = _scrollRect.normalizedPosition;

            // If the position no longer matches what this component last applied, the player
            // scrolled with the wheel, a drag or the scrollbar. Player input wins: stop animating.
            if ((current - _lastAppliedNormalized).sqrMagnitude > SnapEpsilon * SnapEpsilon)
            {
                _isAnimating = false;
                return;
            }

            float t = 1f - Mathf.Exp(-_sharpness * Time.unscaledDeltaTime);
            Vector2 next = Vector2.Lerp(current, _targetNormalized, t);
            if ((next - _targetNormalized).sqrMagnitude < SnapEpsilon * SnapEpsilon)
            {
                next = _targetNormalized;
                _isAnimating = false;
            }

            Apply(next);
        }

        void Apply(Vector2 normalized)
        {
            if (_scrollRect.horizontal) _scrollRect.horizontalNormalizedPosition = normalized.x;
            if (_scrollRect.vertical) _scrollRect.verticalNormalizedPosition = normalized.y;

            _lastAppliedNormalized = _scrollRect.normalizedPosition;
        }

        static float ComputeDelta(float min, float max, float viewMin, float viewMax, float padding, bool keepMaxEdge)
        {
            min -= padding;
            max += padding;

            bool isLargerThanView = max - min > viewMax - viewMin;
            if (isLargerThanView)
                return keepMaxEdge ? viewMax - max : viewMin - min;

            if (max > viewMax)
                return viewMax - max;

            if (min < viewMin)
                return viewMin - min;

            return 0f;
        }

        static float ApplyDelta(float currentNormalized, float delta, float hiddenSize)
        {
            if (hiddenSize <= 0f || Mathf.Approximately(delta, 0f))
                return currentNormalized;

            return Mathf.Clamp01(currentNormalized - delta / hiddenSize);
        }
    }
}