using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class InputButtonUI : MonoBehaviour
{
    [SerializeField] InputActionReference _actionReference;
    [SerializeField] protected Sprite _pressed;
    [SerializeField] protected Sprite _released;
    [SerializeField] protected  Image _graphic;
    [SerializeField] float _step;


    Coroutine _coroutine;
    IEnumerator ShowSpritesCoroutine(List<Sprite> goOver)
    {
        foreach (var sprite in goOver)
        {
            yield return new WaitForSecondsRealtime(_step);

            _graphic.sprite = sprite;
        }
        _coroutine = null;
    }

    void Animate(List<Sprite> frames)
    {
        if (_coroutine != null) StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShowSpritesCoroutine(frames));
    }
}
