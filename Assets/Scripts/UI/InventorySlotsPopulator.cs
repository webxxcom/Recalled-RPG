using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LoadoutView))]
public class InventorySlotsPopulator : MonoBehaviour
{
    [SerializeField] InventorySlot _inventorySlotPrefab;
    [SerializeField] GameObject _parent;

    LoadoutView _loadoutView;
    readonly List<InventoryCell> _createdSlots = new();

    private void Awake()
    {
        _loadoutView = GetComponent<LoadoutView>();

        PopulateView();
    }

    void PopulateView()
    {
        for (int i = 0; i < _loadoutView.Loadout.MaxItemsCount; i++)
        {
            InventoryCell slot = Instantiate(_inventorySlotPrefab, _parent.transform);

            _createdSlots.Add(slot);
        }
    }
}
