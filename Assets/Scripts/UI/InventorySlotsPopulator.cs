using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventorySlotsView))]
public class InventorySlotsPopulator : MonoBehaviour
{
    [SerializeField] InventorySlot _inventorySlotPrefab;
    [SerializeField] GameObject _parent;

    InventorySlotsView _slotsView;
    IReadOnlyList<InventorySlot> _createdSlots;

    private void Awake()
    {
        _slotsView = GetComponent<InventorySlotsView>();

        PopulateView();
        _slotsView.Slots = _createdSlots;
        EventSystem.current.SetSelectedGameObject(_createdSlots[0].gameObject);
    }

    void PopulateView()
    {
        var slots = new List<InventorySlot>();
        for (int i = 0; i < _slotsView.Items.Count; i++)
        {
            InventorySlot slot = Instantiate(_inventorySlotPrefab, _parent.transform);

            slots.Add(slot);
        }
        _createdSlots = slots;
    }
}
