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

        public bool IsActive
        {
            get => _handle != null;
            set
            {
                if (value) Activate();
                else Deactivate();
            }
        }

        GameStateHandle _handle;
        ToggleableGroup _group;
        Toggleable _toggleable;
        GameStateStack _stack;
        Canvas _canvas;
        CanvasGroup _canvasGroup;
        GameObject _prevSelected;

        void Awake()
        {
            _stack = GetComponentInParent<GameStateStack>();
            _group = GetComponentInParent<ToggleableGroup>();
            _toggleable = GetComponent<Toggleable>();
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public bool Activate()
        {
            _handle = _stack.Push(_state);

            if (_handle == null)
                return false;

            _prevSelected = EventSystem.current.currentSelectedGameObject;
            _group.TryAddActive(_toggleable);
            _canvas.enabled = true;
            if (_firstSelected) _firstSelected.Select();
            return true;
        }
        public void Deactivate()
        {
            _stack.Release(_handle);
            _handle = null;

            _group.TryRemoveActive(_toggleable);
            _canvas.enabled = false;
            EventSystem.current.SetSelectedGameObject(_prevSelected);
        }

        public void StopInput()
        {
            _canvasGroup.interactable = false;
        }
        public void ResumeInput()
        {
            _canvasGroup.interactable = true;
        }
    }
}