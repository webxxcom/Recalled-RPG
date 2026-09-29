using System.Text;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    [SerializeField] ItemDefinition _definition;
    [SerializeField] int _count;

    public virtual string Description => Definition.Description;

    public ItemDefinition Definition
    {
        get => _definition;
        private set => _definition = value;
    }

    public int Count
    {
        get => _count;
        set
        {
            if (Definition == null)
            {
                _count = 0;
                return;
            }
            
            _count = Mathf.Clamp(value, Definition.MinStockSize, Definition.MaxStockSize);
            if (value != _count)
                Debug.LogWarning($"Count={value} can't be less than {Definition.MinStockSize} or greater than {Definition.MaxStockSize} " +
                    $"for {GetType().FullName}. Clamping..\n");
        }
    }

    public bool IsEmpty => Definition == default;
    public void SetEmpty() { Definition = default; Count = default; }
    public void SetItem(ItemDefinition def, int count) { Definition = def; Count = count; }

    public ItemInstance()
    {
        SetEmpty();
    }

    public ItemInstance(ItemDefinition itemDefinition, int count = 1)
    {
        SetItem(itemDefinition, count);
    }
}