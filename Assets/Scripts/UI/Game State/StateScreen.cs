using Recalled.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Recalled.UI
{
    [RequireComponent(typeof(Toggleable))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class StateScreen : MonoBehaviour
    {
        [SerializeField] GameStateSO _state;
        [SerializeField] Selectable _firstSelected;

        public bool IsActive => _handle != null;

        GameStateHandle _handle;
        Toggleable _toggleable;
        GameStateStack _stack;
        Canvas _canvas;
        CanvasGroup _canvasGroup;
        GameObject _prevSelected;

        void Awake()
        {
            _stack = GetComponentInParent<GameStateStack>();
            _toggleable = GetComponent<Toggleable>();
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _toggleable.StateChanged.AddListener(OnToggled);
        }

        private void OnEnable() => OnToggled(_toggleable.IsActive);
        private void OnDestroy() => _toggleable.StateChanged.RemoveListener(OnToggled);

        void OnToggled(bool isActive)
        {
            if (isActive) Activate();
            else Deactivate();
        }

        public bool Activate()
        {
            if (IsActive) return true;

            _handle = _stack.Push(_state);

            if (_handle == null)
                return false;

            _prevSelected = EventSystem.current.currentSelectedGameObject;
            _canvas.enabled = true;
            if (_firstSelected) _firstSelected.Select();
            return true;
        }
        public void Deactivate()
        {
            _stack.Release(_handle);
            _handle = null;

            _canvas.enabled = false;
            EventSystem.current.SetSelectedGameObject(_prevSelected);
        }

        public void StopInput() => _canvasGroup.interactable = false;
        public void ResumeInput() => _canvasGroup.interactable = true;
    }
}