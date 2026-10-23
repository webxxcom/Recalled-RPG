using UnityEngine;
using UnityEngine.EventSystems;

namespace Recalled.UI
{
    [RequireComponent(typeof(UISpriteAnimator))]
    [RequireComponent(typeof(Toggleable))]
    public sealed class ToggleableInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        UISpriteAnimator _animator;
        Toggleable _toggleable;

        public void OnPointerClick(PointerEventData eventData) => _toggleable.RequestToggle();

        private void Awake()
        {
            _animator = GetComponent<UISpriteAnimator>();
            _toggleable = GetComponent<Toggleable>();
        }

        private void OnEnable()
        {
            _toggleable.StateChanged += OnToggled;
        }
        private void OnDisable()
        {
            _toggleable.StateChanged -= OnToggled;
        }

        void OnToggled(bool flag)
        {
            if (flag) _animator.Play("Show");
            else _animator.Play("Hide");
        }

        void OnEnter()
        {
            if (!_toggleable.IsActive) _animator.Play("Enter");
        }

        void OnExit()
        {
            if (!_toggleable.IsActive) _animator.Play("Exit");
        }

        public void OnPointerEnter(PointerEventData eventData) => OnEnter();
        public void OnPointerExit(PointerEventData eventData) => OnExit();
    }
}
