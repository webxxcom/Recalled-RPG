using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Player Inventory")]
public class InventorySO : ScriptableObject
{
    [field: SerializeField] public List<ItemInstance> Items { get; private set; }
    [SerializeField] int _maxItemsCount;
    [SerializeField] PopupWorldText _rejectItemText;

    public Sword Sword { get; set; }
    public Armor Armor { get; set; }
    public Boots Boots { get; set; }

    public void UncheckedAdd(ItemInstance ii)
    {
        Items.Add(ii);
    }

    bool TryAddStocked(ItemDefinition itemDefinition, int count)
    {
        if (itemDefinition.MaxStockSize <= 1)
            return false;

        foreach (var iitem in Items)
        {
            if (iitem.Definition == itemDefinition)
            {
                iitem.Count += count;
                return true;
            }
        }
        return false;
    }

    public bool Add(ItemDefinition itemDefinition, int count = 1)
    {
        if (itemDefinition == null || count <= 0)
            return false;

        if (TryAddStocked(itemDefinition, count))
            return true;

        Items.Add(itemDefinition.CreateInstance(count));
        return true;
    }

    public bool Add(ILootable lootable)
    {
        if (lootable == null)
            return false;

        ItemDefinition itemDefinition = lootable.LootTable.GetItem();
        const int count = 1; // Feature for future if loot table will be able to provide more than one item

        if (TryAddStocked(itemDefinition, count))
            return true;

        // Item can't be stocked than check it we can add it
        if (count + Items.Count > _maxItemsCount)
        {
            PopupWorldText pwt = Instantiate(_rejectItemText, lootable.Transform.position, Quaternion.identity);
            pwt.Init($"'{itemDefinition.Name}'\nrejected..");
            return false;
        }

        Items.Add(itemDefinition.CreateInstance(count));
        return true;
    }

    public bool Contains(ItemDefinition item) => Items.Any(ii => ii.Definition == item);

    public void Remove(ItemDefinition item, int count = 1)
    {
        if (count <= 0)
            return;

        foreach (var ii in Items)
        {
            if (ii.Definition == item)
            {
                if (ii.Count - count <= 0)
                    Items.Remove(ii);
                else
                    ii.Count -= count;

                return;
            }
        }
    }

    public void Remove(ItemInstance itemInstance) => Items.Remove(itemInstance);


#if UNITY_EDITOR
    private void OnValidate()
    {
        for (int i = 0; i < Items.Count; i++)
            if (Items[i].Definition != null)
                Items[i] = Items[i].Definition.CreateInstance();
    }
#endif
}
