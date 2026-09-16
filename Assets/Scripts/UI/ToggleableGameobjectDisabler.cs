using UnityEngine;

[RequireComponent(typeof(Toggleable))]
[RequireComponent(typeof(Canvas))]
public class ToggleableGameobjectDisabler : MonoBehaviour
{
    Toggleable _toggleable;
    Canvas _canvas;

    private void Awake()
    {
        _toggleable = GetComponent<Toggleable>();
        _canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        _toggleable.Activated += Enable;
        _toggleable.Deactivated += Disable;
    }
    void OnDisable()
    {
        _toggleable.Activated -= Enable;
        _toggleable.Deactivated -= Disable;
    }
    void Enable()
    {
        if (!_canvas.enabled) _canvas.enabled = true;
        _toggleable.gameObject.SetActive(true);
    }
    void Disable()
    {
        _toggleable.gameObject.SetActive(false);
    }
}
