using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public abstract class UiView : ToggleableObject
{
    Selectable[] _selectables;
    Canvas _canvas;

    protected virtual void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _selectables = GetComponentsInChildren<Selectable>(true);
    }

    void ToggleNavigation(bool isNavigable)
    {
        foreach (var selectable in _selectables)
        {
            selectable.navigation = new Navigation()
            {
                mode = isNavigable ? Navigation.Mode.Automatic : Navigation.Mode.None
            };
        }
    }

    void ToggleElements(bool val)
    {
        gameObject.SetActive(val);
        //_canvas.enabled = val;
        //ToggleNavigation(val);
    }

    protected override void Activate()
    {
        ToggleElements(true);

        EventSystem.current.SetSelectedGameObject(_selectables.Length > 0 ? _selectables[0].gameObject : null);
    }

    protected override void Deactivate()
    {
        ToggleElements(false);
    }
}
