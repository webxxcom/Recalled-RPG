using Recalled.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI
{
    /// <summary>
    /// Frame that follows the selected UI element inside a zone and glides between targets.
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

        [Tooltip("Extra space around the target on each side, in the container's units.")]
        [SerializeField, Min(0f)] float _outline = 1f;

        [Tooltip("How quickly the frame glides to a new target. Higher is snappier.")]
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

            GetComponent<LayoutElement>().ignoreLayout = true;
            _image.raycastTarget = false;
            _image.preserveAspect = true;
            _image.enabled = false;
        }

        void OnEnable() => _currentSelected.Subscribe(ShowAt);

        void OnDisable()
        {
            _currentSelected.Unsubscribe(ShowAt);

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

            if (_isVisible && target == _target)
                return;

            bool wasVisible = _isVisible && _target != null;
            _target = target;
            _isVisible = true;

            transform.SetParent(target.parent, worldPositionStays: true);

            transform.localScale = Vector3.one;
            transform.SetAsLastSibling();

            _animator.Play(IdleAnimation);
            _image.enabled = true;

            if (!wasVisible)
                ApplyLayout(GetTargetPosition(out Vector2 size), size);
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

            Transform container = transform.parent;
            if (transform.GetSiblingIndex() != container.childCount - 1)
                transform.SetAsLastSibling();

            Vector2 targetPosition = GetTargetPosition(out Vector2 targetSize);
            Vector2 position = _rectTransform.localPosition;
            Vector2 size = _rectTransform.rect.size;

            if (position == targetPosition && size == targetSize)
                return;

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

        Vector2 GetTargetPosition(out Vector2 size)
        {
            Rect rect = _target.GetRectInSpace(transform.parent, s_corners);
            Vector2 outline = new(_outline, _outline);

            size = rect.size + 2f * outline;
            Vector2 min = rect.min - outline;

            return min + Vector2.Scale(_rectTransform.pivot, size);
        }

        void ApplyLayout(Vector2 pivotPosition, Vector2 size)
        {
            SetSize(_rectTransform, size);
            _rectTransform.localPosition = pivotPosition;

            if (_imageRect != _rectTransform)
                SetSize(_imageRect, size);
        }

        static void SetSize(RectTransform rectTransform, Vector2 size)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
    }
}