using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(Toggleable))]
public abstract class UiView : MonoBehaviour
{
    Selectable[] _selectables;
    Canvas _canvas;
    protected Toggleable _toggleable;

    protected virtual void Awake()
    {
        _toggleable = GetComponent<Toggleable>();
        _selectables = GetComponentsInChildren<Selectable>(true);
        _canvas = GetComponent<Canvas>();
    }

    protected virtual void Start()
    {
        _toggleable.Activated += Show;
        _toggleable.Deactivated += Hide;
    }

    protected virtual void OnDestroy()
    {
        _toggleable.Activated -= Show;
        _toggleable.Deactivated -= Hide;
    }

    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }

    void ToggleElements(bool val)
    {
        gameObject.SetActive(val);
        _canvas.enabled = val;
    }

    void Show()
    {
        ToggleElements(true);

        EventSystem.current.SetSelectedGameObject(_selectables.Length > 0 ? _selectables[0].gameObject : null);
    }

    void Hide()
    {
        ToggleElements(false);
    }
}
