using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UISpriteAnimator))]
[RequireComponent(typeof(Toggleable))]
public sealed class ToggleableAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{ 
    UISpriteAnimator _animator;
    Toggleable _toggleable;

    public void OnSelect(BaseEventData eventData)
    {
        _toggleable.RequestToggle();
        OnEnter();
    }
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
        if (!_toggleable.IsActive) _animator.Play("Enter");
    }

    void OnExit()
    {
        if (!_toggleable.IsActive) _animator.Play("Exit");
    }

    public void OnPointerEnter(PointerEventData eventData) => OnEnter();
    public void OnPointerExit(PointerEventData eventData) => OnExit();
    public void OnDeselect(BaseEventData eventData) => OnExit();
}
