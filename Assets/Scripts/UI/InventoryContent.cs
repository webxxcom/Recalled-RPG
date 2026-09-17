using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryContent : MonoBehaviour
{
    [SerializeField] InventorySO _inventory;
    [SerializeField] GameObject _content;
    [SerializeField] InventoryCell _inventoryCellPrefab;

    readonly List<InventoryCell> _createdCells = new();

    public bool IsFull => _createdCells.Count(c => c.HasItem) >= _createdCells.Count;

    private void Awake()
    {
        PopulateView();
    }

    private void OnEnable()
    {
        _inventory.OnItemsChanged += RefreshView;

        RefreshView();
    }

    private void OnDisable()
    {
        _inventory.OnItemsChanged -= RefreshView;
    }

    void RefreshView()
    {
        var items = _inventory.GeneralItems;
        for (int i = 0; i < _createdCells.Count; i++)
        {
            if (i < items.Count) _createdCells[i].SetItem(items[i]);
            else _createdCells[i].RemoveItem();
        }
    }

    void ClearView()
    {
        _createdCells.ForEach(ic => Destroy(ic.gameObject));
        _createdCells.Clear();
    }

    void PopulateView()
    {
        var items = _inventory.GeneralItems;
        for (int i = 0; i < _inventory.MaxItemsCount; i++)
        {
            InventoryCell slot = Instantiate(_inventoryCellPrefab, _content.transform);
            _createdCells.Add(slot);

            if (i < items.Count)
                slot.SetItem(items[i]);
        }
    }

    public ItemInstance RemoveItemFromView(InventoryCell cell)
    {
        if (_createdCells.Contains(cell))
        {
            var removed = cell.RemoveItem();
            return removed;
        }
        return null;
    }

    public InventoryCell AddItemToView(ItemInstance item)
    {
        var cell = _createdCells.FirstOrDefault(ic => !ic.HasItem);
        cell.SetItem(item);

        return cell;
    }
}
