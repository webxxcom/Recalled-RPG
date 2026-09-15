using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(UISpriteAnimator))]
public class BookUITab : ToggleableState, IPointerEnterHandler, IPointerExitHandler
{ 
    UISpriteAnimator _animator;
    Button _button;

    public override event Action<ToggleableState> Toggled;

    void Raise() => Toggled?.Invoke(this);
    private void OnEnable() => _button.onClick.AddListener(Raise);
    private void OnDisable() => _button.onClick.RemoveListener(Raise);

    private void Awake()
    {
        _animator = GetComponent<UISpriteAnimator>();
        _button = GetComponent<Button>();
    }

    protected override void Activate()
    {
        _animator.Play("Press");
    }

    protected override void Deactivate()
    {
        _animator.Play("Deactivate");
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
}
