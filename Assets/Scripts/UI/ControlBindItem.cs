using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlBindItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _header;
    [SerializeField] TextMeshProUGUI _bind;
    [SerializeField] Button _rebind;

    public event Action OnBindUpdate;

    ControlsSettingsMenu _section;
    InputAction _inputAction;

    private void OnEnable()
    {
        _rebind.onClick.AddListener(Rebind);
    }
    private void OnDisable()
    {
        _rebind.onClick.RemoveListener(Rebind);
    }

    public void Init(InputAction inputAction, ControlsSettingsMenu section)
    {
        _inputAction = inputAction;
        _section = section;

        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        _bind.text = GetBindingText(_inputAction.bindings[0]);
        _header.text = _inputAction.name;
    }

    string GetBindingText(InputBinding binding)
    {
        string ret = binding.isComposite ? binding.name : binding.ToDisplayString();

        return ret.Length != 0 ? ret : "(No Binding)";
    }

    void Rebind()
    {
        _inputAction.Disable();

        var oper = _inputAction.PerformInteractiveRebinding()
            .WithTargetBinding(0)
            .WithCancelingThrough(Keyboard.current.escapeKey.path)
            .OnMatchWaitForAnother(0.1f);

        _section.NotifyRebinding(oper);
        oper.Start();
        oper.OnComplete(UnblockOnCompletion);
        oper.OnCancel(OnRebindCancel);
    }

    void OnRebindCancel(InputActionRebindingExtensions.RebindingOperation oper)
    {
        oper.action.Enable();
    }

    void UnblockOnCompletion(InputActionRebindingExtensions.RebindingOperation oper)
    {
        oper.action.Enable();
        OnBindUpdate?.Invoke();
    }
}
