using UnityEngine;
using UnityEngine.UI;

public class SettingsToggleField : SettingsConfigField<bool>
{
    [SerializeField] Toggle _toggle;

    protected override bool Value
    {
        get => _toggle.isOn;
        set => _toggle.isOn = value;
    }
    protected override UnityEngine.Events.UnityEvent<bool> Event => _toggle.onValueChanged;
}
