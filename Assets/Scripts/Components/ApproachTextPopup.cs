using TMPro;
using UnityEngine;

public class ApproachTextPopup : MonoBehaviour
{
    [SerializeField] string _displayText;
    [SerializeField] protected TextMeshPro _textMeshPro;

    private void Start()
    {
        _textMeshPro.enabled = false;
        _textMeshPro.text = _displayText;
    }

    public virtual void Show()
    {
        _textMeshPro.enabled = true;
    }

    public virtual void Hide()
    {
        if (_textMeshPro) _textMeshPro.enabled = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_textMeshPro == null)
            _textMeshPro = GetComponentInChildren<TextMeshPro>();
    }
#endif

}
