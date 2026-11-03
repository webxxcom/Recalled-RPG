using System.Collections.Generic;
using System.Linq;
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
        public string Extra => _category.ToString();
        public int MinStockSize => 1;
        public int MaxStockSize { get => _maxStockSize; internal set => _maxStockSize = value; }
        public bool IsStackable => _maxStockSize > MinStockSize;
        public bool IsInCategory(ItemCategory category)
        {
            // If this is general category or the one assigned then fast-return
            if (category == ItemCategory.Any || _category == category)
                return true;

            // Otherwise check the modules extending the category item belongs ot
            return _modules.Any(m => m.DomainCategory == category);
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
