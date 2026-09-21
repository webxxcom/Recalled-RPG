using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
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
    GameObject _currentSelected;

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
        _currentSelected = game;
        transform.SetParent(game.transform);
        transform.SetAsLastSibling();

        var rect = game.GetComponent<RectTransform>().rect;
        _rectTransform.sizeDelta = new(rect.width + 2, rect.height + 2);
        _image.GetComponent<RectTransform>().sizeDelta = _rectTransform.sizeDelta;
    }

    public void Hide()
    {
        _image.enabled = false;
        _animator.Stop();
    }

    private void Update()
    {
        if (_currentSelected != null)
            transform.position = _currentSelected.transform.position;
    }
}
