using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

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

        [SerializeField, Dropdown(nameof(ListBindingPaths))] string _path;
        [SerializeField] StateFrames _frames;

        public string Path => _path;
        public StateFrames Frames => _frames;
    }
    [SerializeField, Dropdown(nameof(ListInputSchemes))] string _schemeName;
    [SerializeField] List<ActionImagePair> _pairs;

    public string Name => _schemeName;
    public List<ActionImagePair> Pairs => _pairs;

    string[] ListInputSchemes()
        => ControlSchemeBindings.ListInputSchemes();
    string[] ListBindingPaths()
        => ControlSchemeBindings.ListAllPathsForScheme(_schemeName);
}