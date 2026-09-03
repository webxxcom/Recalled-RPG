using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextComb : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _multiplier = 100;
    Slider _slider;

    private void Awake()
        => _slider = GetComponentInChildren<Slider>();
    private void Start()
        => OnValChanged(_slider.value);
    private void OnEnable()
        => _slider.onValueChanged.AddListener(OnValChanged);
    private void OnDisable()
        => _slider.onValueChanged.RemoveListener(OnValChanged);

    void OnValChanged(float val)
    {
        if (_multiplier >= 100)
            _text.text = $"{Mathf.RoundToInt(val * _multiplier)}/{_multiplier}";
        else
            _text.text = $"{val * _multiplier:F2}/{_multiplier}";

    }
}
