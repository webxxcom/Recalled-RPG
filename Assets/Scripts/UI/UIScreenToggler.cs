using System.Collections.Generic;
using UnityEngine;

public class UIScreenToggler : MonoBehaviour
{
    [SerializeField] Dictionary<VoidGameEvent, Toggleable> _dictionary;

    private void OnEnable()
    {
        foreach (var pair in _dictionary)
            pair.Key.AddListener(pair.Value.RequestToggle);
    }
    private void OnDisable()
    {
        foreach (var pair in _dictionary)
            pair.Key.RemoveListener(pair.Value.RequestToggle);
    }
}
