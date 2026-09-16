using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UISpriteAnimator))]
public class BookUITab : ToggleableObject, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{ 
    UISpriteAnimator _animator;

    public void OnSelect(BaseEventData eventData)
    {
        RequestToggle();
        OnEnter();
    }
    public void OnPointerClick(PointerEventData eventData) => RequestToggle();

    private void Awake()
    {
        _animator = GetComponent<UISpriteAnimator>();
    }

    protected override void Activate()
    {
        _animator.Play("Press");
    }

    protected override void Deactivate()
    {
        _animator.Play("Hide");
    }

    void OnEnter()
    {
        if (!IsActive) _animator.Play("Enter");
    }

    void OnExit()
    {
        if (!IsActive) _animator.Play("Exit");
    }

    public void OnPointerEnter(PointerEventData eventData) => OnEnter();
    public void OnPointerExit(PointerEventData eventData) => OnExit();
    public void OnDeselect(BaseEventData eventData) => OnExit();
}
