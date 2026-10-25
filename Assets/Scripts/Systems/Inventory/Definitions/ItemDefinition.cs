using System.Collections.Generic;
using UnityEngine;

namespace Recalled.Systems.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Items/General")]
    public class ItemDefinition : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [SerializeField][Min(1)] int _maxStockSize = 999;
        [field: SerializeField] public ItemCategory Category { get; private set; } = ItemCategory.Consumable;
        [SerializeReference, SubclassSelector] List<IModule> _modules;

        public int MinStockSize => 1;
        public int MaxStockSize { get => _maxStockSize; internal set => _maxStockSize = value; }
        public bool IsStackable => _maxStockSize > MinStockSize;
        public bool IsInCategory(ItemCategory category) => category == ItemCategory.Any || Category == category;
        public bool TryGetModule<T>(out T result) where T : IModule
        {
            result = default;
            foreach (var module in _modules)
            {
                if (module is T typed)
                {
                    result = typed;
                    return true;
                }
            }

            return false;
        }

        public virtual ItemInstance CreateInstance(int count = 1)
        {
            return new(this, count);
        }
    }
}
