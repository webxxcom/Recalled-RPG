using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class UIRoot : MonoBehaviour
{
    Canvas _canvas;
    CanvasScaler _scaler;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _scaler = GetComponent<CanvasScaler>();
    }
}
