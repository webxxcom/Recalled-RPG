using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasHider : MonoBehaviour
{
    [SerializeField] float _offset;
    [SerializeField] float _speed = 1.5f;

    CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    Coroutine _hideCoroutine;
    IEnumerator WaitAndHideCanvas()
    {
        while (_canvasGroup.alpha < 0.95f)
        {
            _canvasGroup.alpha += Time.deltaTime * 10;

            yield return null;
        }
        _canvasGroup.alpha = 1;

        yield return new WaitForSeconds(_offset);

        while (_canvasGroup.alpha > 0.1f)
        {
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 0, Time.deltaTime * _speed);

            yield return null;
        }
        _canvasGroup.alpha = 0;
    }

    public void ShowCanvas()
    {
        if (_hideCoroutine != null)
            StopCoroutine(_hideCoroutine);

        _hideCoroutine = StartCoroutine(WaitAndHideCanvas());
    }
}
