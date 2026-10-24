using System.Linq;
using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(ToggleableGroup))]
    sealed class OnlyActiveScreenReceivesInput : MonoBehaviour
    {
        ToggleableGroup _group;
        StateScreen _currentActive;

        private void Awake() => _group = GetComponent<ToggleableGroup>();
        private void Start() => EnsureValidState();
        private void OnEnable() => _group.ActiveChanged += OnActiveChanged;
        private void OnDisable() => _group.ActiveChanged -= OnActiveChanged;

        void OnActiveChanged(IToggleable toggleable)
        {
            var screen = toggleable?.gameObject.GetComponent<StateScreen>();

            SetActiveScreen(screen);
        }

        void EnsureValidState()
        {
            var screens = _group.Elements.Select(t => t.gameObject.GetComponent<StateScreen>());

            foreach (var screen in screens)
                screen.StopInput();

            SetActiveScreen(screens.Where(t => t.IsActive).LastOrDefault());
        }

        void SetActiveScreen(StateScreen stateScreen)
        {
            if (_currentActive != null)
                _currentActive.StopInput();

            _currentActive = stateScreen;
            if (_currentActive != null)
                _currentActive.ResumeInput();
        }
    }
}
