using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game States")]
public class GameStateSO : ScriptableObject
{
    [SerializeField] string[] _actionMaps;
    [SerializeField] CursorLockMode _cursorMode;
    [SerializeField] bool _freezeTime;
    [SerializeField] GameStateSO[] _disallowedAbove = Array.Empty<GameStateSO>();

    public string[] ActionMaps => _actionMaps;
    public CursorLockMode CursorMode => _cursorMode;
    public bool FreezeTime => _freezeTime;

    public bool Disallows(GameStateSO state) => Array.IndexOf(_disallowedAbove, state) >= 0;
}