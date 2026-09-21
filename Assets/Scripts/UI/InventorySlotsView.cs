using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySlotsView : MonoBehaviour
{
    [SerializeField] RuntimeArbitraryList _list;

    IReadOnlyList<InventorySlot> _slots;
    /// <summary>View can be populated with the slots by slots creator</summary>
    public IReadOnlyList<InventorySlot> Slots
    {
        get => _slots;
        set
        {
            _slots = value;
            RefreshView();
        }
    }
    public RuntimeArbitraryList List => _list;

    private void OnEnable()
    {
        _list.ItemsChanged += RefreshView;

        if (Slots != null) RefreshView();
    }

    private void OnDisable()
    {
        _list.ItemsChanged -= RefreshView;
    }

    private void Start()
    {
        Slots ??= GetComponentsInChildren<InventorySlot>();
    }

    void RefreshView()
    {
        var items = _list.Items;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (i < items.Count && !items[i].IsEmpty) Slots[i].SetItem(items[i]);
            else Slots[i].RemoveItem();
        }
    }
}
