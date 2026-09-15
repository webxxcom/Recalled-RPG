using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(UISpriteAnimator))]
public class BookUITab : ToggleableObject, IPointerEnterHandler, IPointerExitHandler
{ 
    UISpriteAnimator _animator;
    Button _button;

    private void OnEnable() => _button.onClick.AddListener(RequestToggle);
    private void OnDisable() => _button.onClick.RemoveListener(RequestToggle);

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
}
