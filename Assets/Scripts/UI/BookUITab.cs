using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BookUITab : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite[] _pressed;
    [SerializeField] Sprite[] _hidden;
    [SerializeField] Sprite _highlighted;
    [SerializeField] Sprite _inactive;
    [SerializeField] Image _graphic;
    [SerializeField] float _step;

    private void Start()
    {
        _graphic.sprite = _inactive;
    }

    void OnPress()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShowSpritesCoroutine(_pressed));
    }

    void OnHide()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShowSpritesCoroutine(_hidden));
    }

    void OnEnter()
    {
        _graphic.sprite = _highlighted;
    }

    void OnExit()
    {
        _graphic.sprite = _inactive;
    }

    Coroutine _coroutine;
    IEnumerator ShowSpritesCoroutine(Sprite[] goOver)
    {
        foreach (var sprite in goOver)
        {
            yield return new WaitForSecondsRealtime(_step);

            _graphic.sprite = sprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData) => OnPress();
    public void OnPointerEnter(PointerEventData eventData) => OnEnter();
    public void OnPointerExit(PointerEventData eventData) => OnExit();
}
