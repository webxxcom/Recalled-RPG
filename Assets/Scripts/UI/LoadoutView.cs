using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadoutView : MonoBehaviour
{
    [SerializeField] InventoryItemCollectionSO _loadout;

    IReadOnlyList<InventorySlot> _slots;
    /// <summary>View can be populated with the slotes with slots creator</summary>
    public IReadOnlyList<InventorySlot> Slots
    {
        get => _slots;
        set
        {
            _slots = value;
            RefreshView();
        }
    }
    public InventoryItemCollectionSO Loadout => _loadout;

    private void OnEnable()
    {
        _loadout.ItemsChanged += RefreshView;

        if (Slots != null) RefreshView();
    }

    private void OnDisable()
    {
        _loadout.ItemsChanged -= RefreshView;
    }

    private void Start()
    {
        Slots ??= GetComponentsInChildren<InventorySlot>();
    }

    void RefreshView()
    {
        var items = _loadout.Items;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (i < items.Count && !items[i].IsEmpty) Slots[i].SetItem(items[i]);
            else Slots[i].RemoveItem();
        }
    }

    public ItemInstance RemoveItemFromView(InventorySlot slot)
    {
        if (Slots.Contains(slot))
        {
            var removed = slot.RemoveItem();
            return removed;
        }
        return null;
    }
}
