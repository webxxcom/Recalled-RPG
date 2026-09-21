using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    [field: SerializeField] public ItemDefinition Definition { get; set; }
    [SerializeField] int _count;

    public virtual string Description => Definition.Description;

    public int Count
    {
        get => _count;
        set
        {
            if (value < 1 || value > Definition.MaxStockSize)
                throw new System.ArgumentException($"Count={value} can't be less than {Definition.MinStockSize} or greater than {Definition.MaxStockSize} for {nameof(ItemInstance)}\n");

            _count = value;
        }
    }

    public bool IsEmpty => Definition == null;
    public void SetEmpty() => Definition = null;
    public void SetItem(ItemDefinition def, int count) { Definition = def; Count = count; }

    public ItemInstance()
    {
        Definition = null;
        _count = 1;
    }

    public ItemInstance(ItemDefinition itemDefinition, int count = 1)
    {
        if (itemDefinition == null)
            throw new System.ArgumentException($"Definition can't be null for {nameof(ItemInstance)}\n");

        Definition = itemDefinition;
        Count = count;
    }
}