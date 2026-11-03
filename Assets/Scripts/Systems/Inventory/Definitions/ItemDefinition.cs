using System.Collections.Generic;
using UnityEngine;

namespace Recalled.Systems.Inventory
{
    [CreateAssetMenu(menuName = "Bag/Item Definition")]
    public sealed class ItemDefinition : ScriptableObject
    {
        [SerializeField] string _name;
        [SerializeField] Sprite _icon;
        [SerializeField] string _description;
        [SerializeField][Min(1)] int _maxStockSize = 999;
        [SerializeField] ItemCategory _category = ItemCategory.Consumable;
        [SerializeReference, SubclassSelector] List<IModule> _modules;

        public string Name => _name;
        public Sprite Icon => _icon;
        public string Description => _description;
        public string Extra => ""; // TODO extra info
        public int MinStockSize => 1;
        public int MaxStockSize { get => _maxStockSize; internal set => _maxStockSize = value; }
        public bool IsStackable => _maxStockSize > MinStockSize;
        public bool IsInCategory(ItemCategory category)
        {
            foreach (var module in _modules)
            {
                if (module.DomainCategory == category)
                    return true;
            }
            return category == ItemCategory.Any || _category == category;
        }

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
        public ItemInstance CreateInstance(int count = 1) => new(this, count);
    }
}
