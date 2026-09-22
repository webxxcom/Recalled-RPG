using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Toggleable))]
public class InventoryFilter : MonoBehaviour
{
    [SerializeField] ItemCategory _category;
    [SerializeField] InventorySlotsView _view;

    Toggleable _toggleable;

    private void Awake()
    {
        _toggleable = GetComponent<Toggleable>();
    }

    private void OnEnable()
    {
        _toggleable.Activated += OnActivated;
        _toggleable.Deactivated += OnDeactivated;
    }
    private void OnDisable()
    {
        _toggleable.Activated -= OnActivated;
        _toggleable.Deactivated -= OnDeactivated;
    }

    void OnActivated()
    {
        _view.Filter = _category;
    }
    void OnDeactivated()
    {
        _view.Filter = ItemCategory.Any;
    }
}
