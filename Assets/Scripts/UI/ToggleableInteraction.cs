using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UISpriteAnimator))]
[RequireComponent(typeof(Toggleable))]
public sealed class ToggleableInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{ 
    [SerializeField] bool _playHovering;
    UISpriteAnimator _animator;
    Toggleable _toggleable;

    public void OnPointerClick(PointerEventData eventData) => _toggleable.RequestToggle();

    private void Awake()
    {
        _animator = GetComponent<UISpriteAnimator>();
        _toggleable = GetComponent<Toggleable>();
    }

    private void OnEnable()
    {
        _toggleable.Activated += OnActivate;
        _toggleable.Deactivated += OnDeactivate;
    }
    private void OnDisable()
    {
        _toggleable.Activated -= OnActivate;
        _toggleable.Deactivated -= OnDeactivate;
    }

    void OnActivate()
    {
        _animator.Play("Press");
    }

    void OnDeactivate()
    {
        _animator.Play("Hide");
    }

    void OnEnter()
    {
        if (_playHovering && !_toggleable.IsActive) _animator.Play("Enter");
    }

    void OnExit()
    {
        if (_playHovering && !_toggleable.IsActive) _animator.Play("Exit");
    }

    public void OnPointerEnter(PointerEventData eventData) => OnEnter();
    public void OnPointerExit(PointerEventData eventData) => OnExit();
}
