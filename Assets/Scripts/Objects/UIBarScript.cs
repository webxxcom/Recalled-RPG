using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarScriptUI : MonoBehaviour
{
    [Tooltip("Do not set if used with Init method")]
    [SerializeField] IntVariable _valueVariable;
    [SerializeField] Image _bar;
    [SerializeField] float _animationSpeed;

    public float MaxValue { get; private set; }
    public float Value { get; private set; }

    public void Init(IntVariable intVariable, int max)
    {
        _valueVariable = intVariable;
        MaxValue = max;

        enabled = true;
    }

    void OnValueChanged(int newValue) => Set(newValue);

    private void OnEnable()
    {
        MaxValue = _valueVariable.Value;
        Set(MaxValue);

        _valueVariable.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
        => _valueVariable.ValueChanged -= OnValueChanged;

    IEnumerator ProgressBars()
    {
        float targetValue = Value / MaxValue;
        while (!Mathf.Approximately(_bar.fillAmount, targetValue))
        {
            _bar.fillAmount
                = Mathf.Lerp(_bar.fillAmount, targetValue, Time.deltaTime * _animationSpeed);
            yield return null;
        }
        _bar.fillAmount = targetValue;
    }

    public void Set(float value)
    {
        Value = Mathf.Clamp(value, 0, MaxValue);

        StopAllCoroutines();
        StartCoroutine(ProgressBars());
    }
}