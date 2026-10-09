using UnityEngine;

namespace Recalled.Systems.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Items/General")]
    public class ItemDefinition : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField][Min(1)] public int MaxStockSize { get; internal set; } = 999;
        [field: SerializeField] public ItemCategory Category { get; private set; } = ItemCategory.Consumable;

        public int MinStockSize => 1;
        public bool IsStackable => MaxStockSize > MinStockSize;

        public virtual ItemInstance CreateInstance(int count = 1)
        {
            return new(this, count);
        }
    }
}
