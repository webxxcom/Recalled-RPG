using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Player Inventory")]
public class InventorySO : ScriptableObject
{
    [field: SerializeField] public List<ItemInstance> Items { get; private set; }
    [SerializeField][Min(1)] int _maxItemsCount;

    public Sword Sword { get; set; }
    public Armor Armor { get; set; }
    public Boots Boots { get; set; }

    public event Action OnItemsChanged;

    public int MaxItemsCount => _maxItemsCount;
    public bool IsFull => Items.Count >= _maxItemsCount;

    public IReadOnlyList<ItemInstance> GeneralItems
        => Items.Where(i => !i.IsQuickSlot).ToArray();

    public void VisualsChanged()
    {
        OnItemsChanged?.Invoke();
    }

    public bool AddItem(ItemInstance item)
    {
        if (item == null)
            throw new InvalidOperationException($"Can't add null item to {nameof(InventorySO)}");
        if (TryAddStocked(item.Definition, item.Count))
            return true;

        if (!IsFull)
        {
            Items.Add(item);
            OnItemsChanged?.Invoke();
            return true;
        }
        return false;
    }

    bool TryAddStocked(ItemDefinition itemDefinition, int count)
    {
        if (!itemDefinition.IsStackable)
            return false;

        foreach (var iitem in Items)
        {
            if (iitem.Definition == itemDefinition)
            {
                int newCount = iitem.Count + count;

                if (newCount > iitem.Definition.MaxStockSize || newCount < iitem.Definition.MinStockSize)
                    continue;

                iitem.Count = newCount;
                OnItemsChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public bool Contains(ItemDefinition item) => Items.Any(ii => ii.Definition == item);

    public bool Remove(ItemDefinition item, int count = 1)
    {
        if (count <= 0)
            return false;

        foreach (var ii in Items)
        {
            if (ii.Definition == item)
            {
                int newCount = ii.Count - count;

                if (newCount == 0)
                    Items.Remove(ii);
                else if (newCount > 0)
                    ii.Count = newCount;
                else
                    continue;

                OnItemsChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public bool Remove(ItemInstance ii) => Remove(ii.Definition, ii.Count);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Items == null)
            return;

        for (int i = 0; i < Items.Count; i++)
        {
            ItemInstance item = Items[i];

            if (item == null)
            {
                Debug.LogWarning($"{name}: entry {i} is null.", this);
                continue;
            }

            ItemDefinition definition = item.Definition;

            if (definition == null)
            {
                Debug.LogWarning($"{name}: entry {i} has no definition.", this);
                continue;
            }

            int min = definition.IsStackable ? definition.MinStockSize : 1;
            int max = definition.IsStackable ? definition.MaxStockSize : 1;

            if (item.Count < min || item.Count > max)
            {
                Debug.LogWarning(
                    $"{name}: '{definition.name}' count {item.Count} is outside [{min}, {max}]. Clamping.", this);
                item.Count = Mathf.Clamp(item.Count, min, max);
            }
        }

        if (Items.Count > _maxItemsCount)
            Debug.LogWarning($"{name}: {Items.Count} entries exceeds Max Items Count ({_maxItemsCount}).", this);
    }
#endif
}
