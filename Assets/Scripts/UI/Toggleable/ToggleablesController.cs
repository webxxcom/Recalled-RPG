using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.UI
{
    public class ToggleablesController : MonoBehaviour
    {
        [SerializeField] ToggleableGroup _activators;
        [SerializeField] ToggleableGroup _activated;
        [SerializeField] Toggleable _firstActivated;

        readonly Dictionary<IToggleable, IToggleable> _activorsToActivated = new();

        private void Awake()
        {
            foreach (var toggleable in _activators.Elements)
                _activorsToActivated[toggleable] = _activated.Elements.FirstOrDefault(t => t.name.Equals(toggleable.name));
        }

        private void Start()
        {
            if (_activators.TryAddActive(_firstActivated))
                ToggleToggleable(_firstActivated);
        }

        private void OnEnable()
        {
            _activators.ActiveChanged += ToggleToggleable;
        }
        private void OnDisable()
        {
            _activators.ActiveChanged -= ToggleToggleable;
        }

        void ToggleToggleable(IToggleable toggleable)
        {
            if (_activorsToActivated.TryGetValue(toggleable, out var value))
                _activated.TryAddActive(value);
        }
    }
}
