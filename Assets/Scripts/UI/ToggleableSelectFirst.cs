using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggleable))]
public sealed class ToggleableSelectFirst : MonoBehaviour
{
    [SerializeField] Selectable _selectFirst;

    Toggleable _toggleable;

    void Awake()
    {
        _toggleable = GetComponent<Toggleable>();
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
        if (_selectFirst != null)
            EventSystem.current.SetSelectedGameObject(_selectFirst.gameObject);
        else
        {
            //var selectable = GetComponentInChildren<Selectable>(true);
            //if (selectable != null)
            //    EventSystem.current.SetSelectedGameObject(selectable.gameObject);
        }
    }
}
