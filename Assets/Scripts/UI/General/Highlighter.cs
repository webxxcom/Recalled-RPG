using Recalled.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI
{
    /// <summary>
    /// Visual that follows the selected UI element inside a zone and glides between targets.
    /// Placement is data: a frame matches the target's size and surrounds it; an arrow keeps its
    /// own size and puts its pivot at a chosen point of the target, plus an offset.
    /// It joins the target's container as the last sibling (not as a child of the target), so it
    /// scrolls with the list, draws above every item and survives the selected item being destroyed.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(LayoutElement))]
    [RequireComponent(typeof(UISpriteAnimator))]
    public sealed class Highlighter : MonoBehaviour
    {
        const string IdleAnimation = "Idle";
        const float SnapDistance = 0.01f;

        [SerializeField] Image _image;
        [SerializeField] Transform _parentZone;

        [Header("Placement")]
        [Tooltip("Match the target's width (plus outline) and cover it horizontally. " +
                 "Off: keep this highlighter's own width and place it using Point On Target.")]
        [SerializeField] bool _matchWidth = true;

        [Tooltip("Match the target's height (plus outline) and cover it vertically. " +
                 "Off: keep this highlighter's own height and place it using Point On Target.")]
        [SerializeField] bool _matchHeight = true;

        [Tooltip("Extra space around the target on each side, in the container's units. " +
                 "Applies only to axes that match the target's size.")]
        [SerializeField, Min(0f)] float _outline = 1f;

        [Tooltip("Point on the target where this highlighter's pivot is placed, normalized like an anchor: " +
                 "(0,0) bottom-left, (1,1) top-right. Used only on axes that don't match the target's size. " +
                 "Arrow to the right of the target: (1, 0.5) with this object's pivot at (0, 0.5).")]
        [SerializeField] Vector2 _pointOnTarget = new(0.5f, 0.5f);

        [Tooltip("Added to the final position, in the container's units. " +
                 "Arrow to the right: (spacing, 0).")]
        [SerializeField] Vector2 _offset = Vector2.zero;

        [Header("Motion")]
        [Tooltip("How quickly the highlighter glides to a new target. Higher is snappier.")]
        [SerializeField, Min(0.01f)] float _sharpness = 20f;

        [Header("Watches")]
        [SerializeField] GameObjectRuntimeVariable _currentSelected;

        // Shared buffer for GetWorldCorners, so measuring never allocates.
        // Safe because UI code runs on the main thread only.
        static readonly Vector3[] s_corners = new Vector3[4];

        UISpriteAnimator _animator;
        RectTransform _rectTransform;
        RectTransform _imageRect;
        RectTransform _target;
        bool _isVisible;

        void Awake()
        {
            _animator = GetComponent<UISpriteAnimator>();
            _rectTransform = (RectTransform)transform;
            _imageRect = _image.rectTransform;

            // The highlighter lives among the target's siblings, so the container's
            // layout group must not position or size it.
            GetComponent<LayoutElement>().ignoreLayout = true;

            // As a sibling drawn on top, the highlighter would otherwise swallow clicks meant for
            // the selected item: pointer events bubble up to parents, never across to siblings.
            _image.raycastTarget = false;

            _image.preserveAspect = true;
            _image.enabled = false;
        }

        void OnEnable() => _currentSelected.Subscribe(ShowAt);

        void OnDisable()
        {
            _currentSelected.Unsubscribe(ShowAt);

            // Hide visually only. Reparenting here would fail when the disable is caused
            // by a parent being deactivated, because Unity forbids hierarchy changes then.
            Hide();
        }

        public void ShowAt(GameObject selected)
        {
            if (selected == null) Hide();
            else Show(selected);
        }

        void Show(GameObject selected)
        {
            if (!selected.transform.IsChildOf(_parentZone) || selected.transform is not RectTransform target)
                return;

            // Ignore repeated notifications for the same target, so the idle animation doesn't restart.
            if (_isVisible && target == _target)
                return;

            bool wasVisible = _isVisible && _target != null;
            _target = target;
            _isVisible = true;

            // Keep the current on-screen position so the highlighter can glide from where it is.
            transform.SetParent(target.parent, worldPositionStays: true);

            // worldPositionStays may compensate scale when containers differ in scale;
            // sizes and offsets are in the container's units, so the highlighter must have a scale of one.
            transform.localScale = Vector3.one;
            transform.SetAsLastSibling();

            _animator.Play(IdleAnimation);
            _image.enabled = true;

            // Appearing from hidden: snap into place instead of flying in from a stale position.
            if (!wasVisible)
                ApplyLayout(GetTargetPlacement(out Vector2 size), size);
        }

        void Hide()
        {
            _isVisible = false;
            _target = null;
            _image.enabled = false;
            _animator.Stop();
        }

        void LateUpdate()
        {
            if (!_isVisible)
                return;

            // Unity-null: the target was destroyed, for example when its item was consumed and the slot removed.
            if (_target == null)
            {
                Hide();
                return;
            }

            // Items instantiated into the container after the highlighter joined it would draw on top of it.
            Transform container = transform.parent;
            if (transform.GetSiblingIndex() != container.childCount - 1)
                transform.SetAsLastSibling();

            // Track the target every frame, so the highlighter also follows layout changes
            // and a target that moves inside its container.
            Vector2 targetPosition = GetTargetPlacement(out Vector2 targetSize);
            Vector2 position = _rectTransform.localPosition;
            Vector2 size = _rectTransform.rect.size;

            // Already in place: don't touch the RectTransform, since changing it
            // marks the canvas for a rebuild. On kept-size axes targetSize equals size.
            if (position == targetPosition && size == targetSize)
                return;

            // Exponential decay is frame-rate independent. Unscaled time keeps it working
            // in menus that pause the game by setting Time.timeScale to 0.
            float t = 1f - Mathf.Exp(-_sharpness * Time.unscaledDeltaTime);
            position = Vector2.Lerp(position, targetPosition, t);
            size = Vector2.Lerp(size, targetSize, t);

            if ((position - targetPosition).sqrMagnitude < SnapDistance * SnapDistance &&
                (size - targetSize).sqrMagnitude < SnapDistance * SnapDistance)
            {
                position = targetPosition;
                size = targetSize;
            }

            ApplyLayout(position, size);
        }

        /// <summary>
        /// Where the highlighter's pivot must sit, and how large it must be, for the current target.
        /// Both are in the container's local space. Axes that keep their own size return the current size.
        /// </summary>
        Vector2 GetTargetPlacement(out Vector2 size)
        {
            Rect rect = _target.GetRectInSpace(transform.parent, s_corners);
            Vector2 pivot = _rectTransform.pivot;
            Vector2 ownSize = _rectTransform.rect.size;

            size = new Vector2(
                _matchWidth ? rect.width + 2f * _outline : ownSize.x,
                _matchHeight ? rect.height + 2f * _outline : ownSize.y);

            Vector2 position = new(
                PlaceOnAxis(_matchWidth, rect.xMin, rect.width, pivot.x, size.x, _pointOnTarget.x),
                PlaceOnAxis(_matchHeight, rect.yMin, rect.height, pivot.y, size.y, _pointOnTarget.y));

            return position + _offset;
        }

        /// <summary>
        /// Pivot position on one axis.
        /// Covering: the highlighter spans the target plus outline, so its pivot sits pivot * size
        /// from its own min edge. That keeps a frame correct whatever pivot it uses.
        /// Not covering: the pivot sits at the chosen normalized point of the target.
        /// </summary>
        float PlaceOnAxis(bool covers, float targetMin, float targetSize, float pivot, float size, float point)
        {
            return covers
                ? targetMin - _outline + pivot * size
                : targetMin + point * targetSize;
        }

        void ApplyLayout(Vector2 pivotPosition, Vector2 size)
        {
            // Only matched axes are resized; a kept size (an arrow) is never written.
            SetSize(_rectTransform, size);
            if (_imageRect != _rectTransform)
                SetSize(_imageRect, size);

            _rectTransform.localPosition = pivotPosition;
        }

        void SetSize(RectTransform rectTransform, Vector2 size)
        {
            // SetSizeWithCurrentAnchors gives the requested size whatever the anchors are,
            // unlike writing sizeDelta, which equals the size only when the anchors meet at one point.
            if (_matchWidth) rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            if (_matchHeight) rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
    }
}