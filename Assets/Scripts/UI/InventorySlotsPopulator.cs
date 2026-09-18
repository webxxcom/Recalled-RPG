using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventorySlotsView))]
public class InventorySlotsPopulator : MonoBehaviour
{
    [SerializeField] InventorySlot _inventorySlotPrefab;
    [SerializeField] GameObject _parent;

    InventorySlotsView _loadoutView;
    readonly List<InventoryCell> _createdSlots = new();

    private void Awake()
    {
        _loadoutView = GetComponent<InventorySlotsView>();

        PopulateView();
    }

    void PopulateView()
    {
        for (int i = 0; i < _loadoutView.List.Items.Count; i++)
        {
            InventoryCell slot = Instantiate(_inventorySlotPrefab, _parent.transform);

            _createdSlots.Add(slot);
        }
    }
}
