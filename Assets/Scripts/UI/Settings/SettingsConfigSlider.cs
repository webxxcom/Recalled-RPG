using UnityEngine;
using UnityEngine.UI;

public class SettingsConfigSlider : MonoBehaviour, ISettingsConfigField<Slider>
{
    public Slider Serialized { get; set; }
    public bool Changed { get; set; }

    private void Awake()
        => Serialized = GetComponentInChildren<Slider>();
    private void OnEnable()
        => Serialized.onValueChanged.AddListener(ValueChanged);
    void ValueChanged(float _)
        => Changed = true;
}
