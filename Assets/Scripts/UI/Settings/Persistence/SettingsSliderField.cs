using UnityEngine;
using UnityEngine.UI;

public class SettingsSliderField : SettingsConfigField<float>
{
    [SerializeField] Slider _slider;

    protected override float Value
    {
        get => _slider.normalizedValue;
        set => _slider.normalizedValue = value;
    }
    protected override UnityEngine.Events.UnityEvent<float> Event => _slider.onValueChanged;
}
