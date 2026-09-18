using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UISpriteAnimator))]
[RequireComponent(typeof(RectTransform))]
public class Highlighter : MonoBehaviour
{
    [Header("Listens to")]
    [SerializeField] GameobjectGameEvent _selectableChanged;
    [SerializeField] Image _image;

    UISpriteAnimator _animator;
    RectTransform _rectTransform;

    private void Awake()
    {
        _animator = GetComponent<UISpriteAnimator>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _selectableChanged.AddListener(Show);
    }
    private void OnDisable()
    {
        _selectableChanged.RemoveListener(Show);
    }

    public void Show(GameObject game)
    {
        _animator.Play("Idle");
        _image.enabled = true;
        transform.position = game.transform.position;

        var rect = game.GetComponent<RectTransform>().rect;
        _rectTransform.sizeDelta = new(rect.width + 2, rect.height + 2);
        _image.GetComponent<RectTransform>().sizeDelta = _rectTransform.sizeDelta;
    }

    public void Hide()
    {
        _image.enabled = false;
        _animator.Stop();
    }
}
