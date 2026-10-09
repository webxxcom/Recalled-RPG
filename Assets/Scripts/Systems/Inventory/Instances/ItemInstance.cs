using System;
using UnityEngine;

namespace Recalled.Systems.Inventory
{
    [System.Serializable]
    public class ItemInstance
    {
        [SerializeField] ItemDefinition _definition;
        [SerializeField] int _count;

        public ItemDefinition Definition => _definition;
        public int Count => _count;
        public virtual string Description => _definition.Description;

        public ItemInstance(ItemDefinition definition, int count)
        {
            if (definition == null)
                throw new ArgumentNullException();
            if (count <= 0 || (!definition.IsStackable && count != 1) || count > definition.MaxStockSize)
                throw new ArgumentException();

            _definition = definition;
            _count = count;
        }

        public int AppendStock(int count) => SetStock(_count + count);
        public int SetStock(int count)
        {
            _count = Mathf.Clamp(count, _definition.MinStockSize, _definition.MaxStockSize);
            return count - _count;
        }
    }
}
