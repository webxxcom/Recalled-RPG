using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField][Min(1)] public int MaxStockSize { get; private set; } = 999;

    public int MinStockSize => 1;
    public bool IsStockable => MaxStockSize > MinStockSize;

    public virtual ItemInstance CreateInstance(int count = 1)
    {
        if (count > MaxStockSize)
            throw new System.ArgumentException($"{Name}'s quantity of {count} exceeded the allowed ${MaxStockSize}");

        return new(this, count);
    }
}
