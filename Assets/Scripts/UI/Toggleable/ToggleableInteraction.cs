using UnityEngine;
using UnityEngine.EventSystems;

namespace Recalled.UI
{
    [RequireComponent(typeof(UISpriteAnimator))]
    [RequireComponent(typeof(Toggleable))]
    sealed class ToggleableInteraction
        : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        UISpriteAnimator _animator;
        Toggleable _toggleable;

        private void Awake()
        {
            _animator = GetComponent<UISpriteAnimator>();
            _toggleable = GetComponent<Toggleable>();
        }
        private void OnEnable()
        {
            _toggleable.StateChanged.AddListener(UpdateState);

            UpdateState(_toggleable.IsActive);
        }
        private void OnDisable() => _toggleable.StateChanged.RemoveListener(UpdateState);

        void UpdateState(bool flag)
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
        public void OnPointerClick(PointerEventData eventData) => _toggleable.RequestToggle();
        public void OnPointerEnter(PointerEventData eventData) => OnEnter();
        public void OnPointerExit(PointerEventData eventData) => OnExit();
    }
}
