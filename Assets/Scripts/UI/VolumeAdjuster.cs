using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeAdjuster : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] AudioMixer _mixer;
    [SerializeField] string _varName;

    private void Awake()
    {
        if (_mixer.GetFloat(_varName, out var val))
            _slider.value = Mathf.Approximately(val, -80) ? 0f : Mathf.Pow(10, val / 20);
    }

    private void OnEnable()
        => _slider.onValueChanged.AddListener(OnValChanged);
    private void OnDisable()
        => _slider.onValueChanged.RemoveListener(OnValChanged);
    void OnValChanged(float val)
        => _mixer.SetFloat(_varName, Mathf.Approximately(val, 0) ? -80f : Mathf.Log10(val) * 20f);
}
