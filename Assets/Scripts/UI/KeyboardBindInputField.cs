using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyboardBindInputField : MonoBehaviour
{
    [SerializeField] InputAction _action;
    [SerializeField] Button _saveButton;
    [SerializeField] Button _bindButton;

    private void OnEnable()
    {
        _bindButton.onClick.AddListener(OnRebinding);
    }
    InputActionRebindingExtensions.RebindingOperation rebinding;
    void OnRebinding()
    {
        rebinding = _action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithTargetBinding(0)
            .OnMatchWaitForAnother(0.1f);
        rebinding.Start();
        rebinding.OnComplete(OnRebindingComplete);
    }

    void OnRebindingComplete(InputActionRebindingExtensions.RebindingOperation oper)
    {
        oper.action.Enable();

        rebinding.Cancel();
    }

    void Save()
    {
    }
}
