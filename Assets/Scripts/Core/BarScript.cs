using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    [SerializeField] ValueResource _valueResource;
    [SerializeField] Image _topBar;
    [SerializeField, Tooltip("Optional")] Image _bottomBar;
    [SerializeField] float _animationSpeed;

    public UnityEvent ProgressStarted;
    public UnityEvent ProgressEnded;

    public float Value => _valueResource.CurrentValue;
    public float MaxValue => _valueResource.MaxValue;

    void OnValueChanged(int _, int newValue) => Set(newValue);

    private void OnEnable()
    {
        Set(_valueResource.CurrentValue);

        _valueResource.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
        => _valueResource.ValueChanged -= OnValueChanged;

    IEnumerator ProgressBars(float delta)
    {
        var targetValue = Value / MaxValue;
        var suddenBar = delta <= 0 ? _topBar : _bottomBar;
        var smoothBar = delta <= 0 ? _bottomBar : _topBar;

        ProgressStarted.Invoke();
        if (suddenBar) suddenBar.fillAmount = targetValue;
        while (Mathf.Abs(targetValue - smoothBar.fillAmount) > 0.01f)
        {
            smoothBar.fillAmount
                = Mathf.Lerp(smoothBar.fillAmount, targetValue, Time.deltaTime * _animationSpeed);
            yield return null;
        }
        smoothBar.fillAmount = targetValue;
        ProgressEnded.Invoke();
    }

    public void Set(float value)
    {
        float prevValue = Value;

        StopAllCoroutines();
        StartCoroutine(ProgressBars(value - prevValue));
    }
}