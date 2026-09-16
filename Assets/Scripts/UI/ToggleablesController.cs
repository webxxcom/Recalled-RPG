using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToggleablesController : MonoBehaviour
{
    [SerializeField] ToggleableGroup _activators;
    [SerializeField] ToggleableGroup _activated;
    [SerializeField] ToggleableObject _firstActivated;
    [SerializeField] Dictionary<ToggleableObject, ToggleableObject> _activorsToActivated = new();

    private void Start()
    {
        if (_activorsToActivated.Count == 0)
        {
            foreach (var toggleable in _activators.Elements)
                _activorsToActivated[toggleable] = _activated.Elements.FirstOrDefault(t => t.name.Equals(toggleable.name));
        }

        if (!_activators.RequestScreen(_firstActivated))
            Debug.LogError($"Couldn't toggle {nameof(_firstActivated)} on {nameof(ToggleablesController)}");
    }

    private void OnEnable()
    {
        _activators.ScreenChanged += OnToggleableToggled;
    }
    private void OnDisable()
    {
        _activators.ScreenChanged -= OnToggleableToggled;
    }

    void OnToggleableToggled(ToggleableObject toggleable)
    {
        if (_activorsToActivated.TryGetValue(toggleable, out var value))
        {
            if (!_activated.RequestScreen(value))
                throw new System.Exception($"Invalid operation of toggling a {nameof(ToggleableObject)} on {nameof(ToggleablesController)}");
        }
    }
}
