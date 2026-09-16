using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleablesController : MonoBehaviour
{
    [SerializeField] ToggleableGroup _activators;
    [SerializeField] ToggleableGroup _activated;
    [SerializeField] Toggleable _firstActivated;

    Dictionary<Toggleable, Toggleable> _activorsToActivated;

    private void Awake()
    {
        _activorsToActivated = new();
        foreach (var toggleable in _activators.Elements)
            _activorsToActivated[toggleable] = _activated.Elements.FirstOrDefault(t => t.name.Equals(toggleable.name));
    }

    private void Start()
    {
        if (_activators.Request(_firstActivated))
            ToggleToggleable(_firstActivated);
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
            _activated.Request(value);
    }
}
