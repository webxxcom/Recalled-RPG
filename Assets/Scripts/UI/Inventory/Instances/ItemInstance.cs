using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    [field: SerializeField] public ItemDefinition Definition { get; private set; }
    [SerializeField] int _count;

    public virtual string Description => Definition.Description;

    public int Count
    {
        get => _count;
        set
        {
            if (value < 1 || value > Definition.MaxStockSize)
                throw new System.ArgumentException($"Count can't be less than 1 for {nameof(ItemInstance)}\n");

            _count = value;
        }
    }

    public ItemInstance(ItemDefinition itemDefinition, int count = 1)
    {
        if (itemDefinition == null)
            throw new System.ArgumentException($"Definition can't be null for {nameof(ItemInstance)}\n");

        Definition = itemDefinition;
        Count = count;
    }
}