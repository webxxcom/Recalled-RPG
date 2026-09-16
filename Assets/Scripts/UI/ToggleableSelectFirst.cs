using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggleable))]
public sealed class ToggleableSelectFirst : MonoBehaviour
{
    Selectable[] _selectables;
    Toggleable _toggleable;

    void Awake()
    {
        _toggleable = GetComponent<Toggleable>();
        _selectables = GetComponentsInChildren<Selectable>(true);
    }

    void OnEnable()
    {
        _toggleable.Activated += Show;
    }

    void OnDisable()
    {
        _toggleable.Activated -= Show;
    }

    void Show()
    {
        EventSystem.current.SetSelectedGameObject(_selectables.Length > 0 ? _selectables[0].gameObject : null);
    }
}
