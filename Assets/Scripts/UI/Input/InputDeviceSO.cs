using System;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "UI/Input Device")]
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
    [SerializeField] string _deviceName;
    [SerializeField] List<ActionImagePair> _binds;
    public List<ActionImagePair> Binds => _binds;

    public bool HaveSamePath(ActionImagePair aip, InputBinding binding)
        => $"<{_deviceName}>/{aip.Path.ToLower()}".Equals(binding.path);

    string[] ShowBindingPaths()
    {
        if (_deviceName == null)
            return Array.Empty<string>();

        var a = InputSystem.devices.First(d => d.displayName.Equals(_deviceName));
        if (a == null)
        {
            Debug.Log($"Incorrect Input device name for {nameof(InputDeviceSO)}");
            return Array.Empty<string>();
        }

        return a.allControls.Select(c => c.displayName).ToArray();
    }

}