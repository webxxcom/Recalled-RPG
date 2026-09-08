using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Settings.Persistence
{
    public class SettingsSliderField : SettingsConfigField<float>
    {
        [SerializeField] Slider _slider;

        protected override float Value
        {
            get => _slider.normalizedValue;
            set => _slider.normalizedValue = value;
        }
        protected override UnityEvent<float> Event => _slider.onValueChanged;
    }
}
