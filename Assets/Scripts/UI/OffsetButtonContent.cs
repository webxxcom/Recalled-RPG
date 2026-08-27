using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class ButtonPressOffset : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, ISubmitHandler
{
    [SerializeField] RectTransform _content;
    [SerializeField] Vector2 _pressedOffset = new(0f, -3f);
    [SerializeField] float _submitHoldDuration = 0.08f;

    Selectable _selectable;
    Vector2 _restPosition;
    Coroutine _submitRoutine;

    private void Awake()
    {
        _selectable = GetComponent<Selectable>();
        _restPosition = _content.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_selectable.IsInteractable())
            return;

        SetPressed(true);
    }

    public void OnPointerUp(PointerEventData eventData)
        => SetPressed(false);

    public void OnSubmit(BaseEventData eventData)
    {
        if (!_selectable.IsInteractable())
            return;

        if (_submitRoutine != null)
            StopCoroutine(_submitRoutine);
        _submitRoutine = StartCoroutine(SubmitDip());
    }

    private IEnumerator SubmitDip()
    {
        SetPressed(true);

        yield return new WaitForSecondsRealtime(_submitHoldDuration);

        SetPressed(false);
        _submitRoutine = null;
    }

    private void SetPressed(bool pressed)
    {
        if (_content == null)
            return;

        _content.anchoredPosition = pressed
            ? _restPosition + _pressedOffset
            : _restPosition;
    }
}