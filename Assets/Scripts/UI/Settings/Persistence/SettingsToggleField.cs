using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Settings.Persistence
{
    public class SettingsToggleField : SettingsConfigField<bool>
    {
        [SerializeField] Toggle _toggle;

        protected override bool Value
        {
            get => _toggle.isOn;
            set => _toggle.isOn = value;
        }
        protected override UnityEvent<bool> Event => _toggle.onValueChanged;
    }
}
