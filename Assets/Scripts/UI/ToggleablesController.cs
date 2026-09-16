using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleablesController : MonoBehaviour
{
    [SerializeField] ToggleableGroup _activators;
    [SerializeField] ToggleableGroup _activated;
    [SerializeField] Toggleable _firstActivated;
    [SerializeField] Dictionary<Toggleable, Toggleable> _activorsToActivated = new();

    private void Start()
    {
        if (_activorsToActivated.Count == 0)
        {
            foreach (var toggleable in _activators.Elements)
                _activorsToActivated[toggleable] = _activated.Elements.FirstOrDefault(t => t.name.Equals(toggleable.name));
        }

        if (!_activators.Request(_firstActivated))
            Debug.LogError($"Couldn't toggle {nameof(_firstActivated)} on {nameof(ToggleablesController)}");
    }

    private void OnEnable()
    {
        _activators.ScreenChanged += ToggleToggleable;
    }
    private void OnDisable()
    {
        _activators.ScreenChanged -= ToggleToggleable;
    }

    void ToggleToggleable(Toggleable toggleable)
    {
        if (_activorsToActivated.TryGetValue(toggleable, out var value))
        {
            if (!_activated.Request(value))
                Debug.LogError($"Invalid operation of toggling a {nameof(Toggleable)} on {nameof(ToggleablesController)}");
        
        }
    }
}
