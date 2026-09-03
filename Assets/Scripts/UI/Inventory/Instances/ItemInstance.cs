using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    [field: SerializeField] public ItemDefinition Definition { get; private set; }
    [field: SerializeField] public int Count { get; set; }

    public virtual string Description => Definition.Description;

    public ItemInstance(ItemDefinition itemDefinition, int count = 1)
    {
        Definition = itemDefinition;
        Count = count;
    }
}