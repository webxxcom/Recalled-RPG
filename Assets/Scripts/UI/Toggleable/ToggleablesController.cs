using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.UI
{
    internal sealed class ToggleablesController : MonoBehaviour
    {
        [SerializeField] ToggleableGroup _activators;
        [SerializeField] ToggleableGroup _activatees;

        readonly Dictionary<IToggleable, Toggleable> _activorsToActivated = new();

        private void Awake()
        {
            foreach (var toggleable in _activators.Elements)
            {
                var found = _activatees.Elements.FirstOrDefault(t => t.name.Equals(toggleable.name));

                if (found != null)
                    _activorsToActivated[toggleable] = found;
            }
        }
        private void OnEnable()
        {
            _activators.ActiveChanged += ToggleActivatee;

            var activeActivator = _activators.Elements.FirstOrDefault(t => t.IsActive);
            if (activeActivator) ToggleActivatee(activeActivator);
        }
        private void OnDisable() => _activators.ActiveChanged -= ToggleActivatee;

        void ToggleActivatee(IToggleable toggleable)
        {
            if (_activorsToActivated.TryGetValue(toggleable, out var value))
                value.RequestToggle();
        }
    }
}
