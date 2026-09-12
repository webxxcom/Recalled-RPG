using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "UI/Input Device Mapping")]
public class InputDeviceSO : ScriptableObject
{
    [System.Serializable]
    public class ActionImagePair
    {
        [System.Serializable]
        public class StateFrames
        {
            [SerializeField] public Sprite Pressed;
            [SerializeField] public Sprite Released;
        }

        [SerializeField, Dropdown(nameof(ShowBindingPaths))] string _path;
        [SerializeField] StateFrames _frames;

        public string Path => _path;
        public StateFrames Frames => _frames;
    }
    [SerializeField, Dropdown(nameof(AvailableDevices))] string _schemeName;
    [SerializeField] List<ActionImagePair> _pairs;
    public string Name => _schemeName;
    public List<ActionImagePair> Pairs => _pairs;

    string[] AvailableDevices()
        => InputSystem.actions.controlSchemes.Select(cs => cs.name).ToArray();

    string[] ShowBindingPaths()
    {
        if (_schemeName == null)
            return Array.Empty<string>();

        string[] schemes = _schemeName.Split("&");

        var inputDevices = InputSystem.devices.Where(d => schemes.Any(s => d.name.Contains(s)));
        if (inputDevices.Count() == 0)
        {
            Debug.Log($"Incorrect Input device name for {nameof(InputDeviceSO)}.{name}");
            return Array.Empty<string>();
        }
        return inputDevices.SelectMany(d => d.allControls.Select(d => d.name)).ToArray();
    }

}