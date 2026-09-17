using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows a slider's normalized value as a whole percentage ("75%").
/// </summary>
public class SliderPercentText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Slider _slider;

    void OnEnable()
    {
        _slider.onValueChanged.AddListener(OnValChanged);
        OnValChanged(_slider.value);
    }
    void OnDisable()
        => _slider.onValueChanged.RemoveListener(OnValChanged);

    void OnValChanged(float _)
        => _text.text = Mathf.RoundToInt(_slider.normalizedValue * 100f) + "%";

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_slider == null) _slider = GetComponentInChildren<Slider>();
        if (_text == null) _text = GetComponentInChildren<TextMeshProUGUI>();
    }
#endif
}
