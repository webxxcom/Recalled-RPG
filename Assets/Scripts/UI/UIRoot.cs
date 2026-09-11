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

        _canvas.referencePixelsPerUnit = _scaler.referencePixelsPerUnit;
        Debug.Log($"UIRoot ran and i set {_scaler.referencePixelsPerUnit} PPU from scaler");

        foreach (Canvas c in GetComponentsInChildren<Canvas>(true))
        {
            Debug.Log($"Setting {c.name} PPU to {_scaler.referencePixelsPerUnit}");
            c.referencePixelsPerUnit = _scaler.referencePixelsPerUnit;
            Debug.Log($"Now {c.name} has ${c.referencePixelsPerUnit} PPU");
        }
    }
}
