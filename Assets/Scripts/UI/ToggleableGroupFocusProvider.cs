using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI
{
    [RequireComponent(typeof(ToggleableGroup))]
    public class ToggleableGroupFocusProvider : MonoBehaviour, IScreenFocusProvider
    {
        ToggleableGroup _group;
        public event Action FocusTargetChanged;

        private void Awake() => _group = GetComponent<ToggleableGroup>();
        void OnEnable()
        {
            foreach (var toggleable in _group.Elements)
                toggleable.GetComponent<IScreenFocusProvider>().FocusTargetChanged += Raise;
        }
        void OnDisable()
        {
            foreach (var toggleable in _group.Elements)
                toggleable.GetComponent<IScreenFocusProvider>().FocusTargetChanged -= Raise;
        }
        void Raise() => FocusTargetChanged?.Invoke();

        public void OnFocused(Selectable selected)
        {
            var firstActive = _group.Elements.FirstOrDefault(t => t.IsActive);
            if (firstActive && firstActive.TryGetComponent<IScreenFocusProvider>(out var provider))
                provider.OnFocused(selected);
        }

        public bool TryGetFocusTarget(out Selectable target)
        {
            target = null;
            var firstActive = _group.Elements.FirstOrDefault(t => t.IsActive);
            if (firstActive)
                return firstActive.TryGetComponent<IScreenFocusProvider>(out var provider)
                    && provider.TryGetFocusTarget(out target);

            return false;
        }
    }
}
