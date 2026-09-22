using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UISpriteAnimator))]
[RequireComponent(typeof(RectTransform))]
public class Highlighter : MonoBehaviour
{
    [SerializeField] Image _image;
    [Header("Watches"), SerializeField] GameObjectRuntimeVariable _currentSelected;

    UISpriteAnimator _animator;
    RectTransform _rectTransform;

    private void Awake()
    {
        _animator = GetComponent<UISpriteAnimator>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _currentSelected.ValueChanged += SetTo;
    }
    private void OnDisable()
    {
        _currentSelected.ValueChanged -= SetTo;
    }

    void Show(GameObject game)
    {
        _animator.Play("Idle");
        _image.enabled = true;
        transform.position = game.transform.position;
        transform.SetParent(game.transform);
        transform.SetAsLastSibling();

        var rect = game.GetComponent<RectTransform>().rect;
        _rectTransform.sizeDelta = new(rect.width + 2, rect.height + 2);
        _image.GetComponent<RectTransform>().sizeDelta = _rectTransform.sizeDelta;
    }

    void Hide()
    {
        _image.enabled = false;
        _animator.Stop();
    }

    public void SetTo(GameObject game)
    {
        if (game == null) Hide();
        else Show(game);
    }

    private void Update()
    {
        if (_currentSelected.Value != null)
            transform.position = _currentSelected.Value.transform.position;
    }
}
