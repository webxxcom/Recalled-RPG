using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextComb : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Slider _slider;

    void OnValChanged(float val)
    {
        _text.text = val.ToString("F0");
    }

    /// <summary>
    /// Function which can be used on Slider's 'On Value Changed' event to edit the text on scene view
    /// </summary>
    public void UpdateText()
    {
        OnValChanged(_slider.value);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_slider == null) _slider = GetComponentInChildren<Slider>();
    }
#endif
}
