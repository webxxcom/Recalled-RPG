using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Recalled.UI
{
    [RequireComponent(typeof(ToggleableGroup))]
    internal sealed class FocusManager : MonoBehaviour
    {
        [SerializeField] InputDeviceRuntimeVariable _currentInputDevice;
        [SerializeField] InputDeviceSO _gamepadDevice;

        ScreenBehavior _top;
        GameObject _lastSeen;
        ToggleableGroup _group;

        private void Awake() => _group = GetComponent<ToggleableGroup>();

        void OnEnable()
        {
            _group.ActiveChanged += OnScreenChanged;

            var active = _group.Elements.FirstOrDefault(t => t.IsActive);
            if (active != null) OnScreenChanged(active);
        }
        void OnDisable() => _group.ActiveChanged -= OnScreenChanged;
        void OnScreenChanged(IToggleable toggleable)
            => SetTopScreen(toggleable?.gameObject.GetComponent<ScreenBehavior>());

        public void SetTopScreen(ScreenBehavior screen)
        {
            if (_top != null) _top.FocusTargetChanged -= OnTargetChanged;
            _top = screen;
            if (_top != null) _top.FocusTargetChanged += OnTargetChanged;

            OnTargetChanged();
        }

        public void EnsureFocus()
        {
            if (_top == null || IsValid(Current())) return;

            if (_top.TryGetFocusTarget(out var target) && IsValid(target))
                EventSystem.current.SetSelectedGameObject(target.gameObject);
            else EventSystem.current.SetSelectedGameObject(null);
        }

        void OnTargetChanged()
        {
            EnsureFocus();
        }

        void LateUpdate()
        {
            var es = EventSystem.current;
            var current = es != null ? es.currentSelectedGameObject : null;
            if (current == _lastSeen) return;
            _lastSeen = current;

            if (_top != null && current != null && current.TryGetComponent<Selectable>(out var s) && IsValid(s))
                _top.OnFocused(s);
        }

        Selectable Current()
        {
            var es = EventSystem.current;
            var go = es != null ? es.currentSelectedGameObject : null;
            return go != null && go.TryGetComponent<Selectable>(out var s) ? s : null;
        }

        bool IsValid(Selectable s) =>
            s != null && s.isActiveAndEnabled && s.IsInteractable() && s.transform.IsChildOf(_top.transform);

        bool IsGamepadActive() => _currentInputDevice.Value == _gamepadDevice;
    }
}
