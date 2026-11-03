using System;
using UnityEngine;

namespace Recalled.Systems.Inventory
{
    [Serializable]
    public sealed class ItemInstance
    {
        [SerializeField] ItemDefinition _definition;
        [SerializeField, Min(1)] int _count;

        public ItemDefinition Definition => _definition;
        public int Count => _count;

        internal ItemInstance(ItemDefinition definition, int count)
        {
            if (definition == null)
                throw new ArgumentNullException();
            if (count <= 0 || (!definition.IsStackable && count != definition.MinStockSize) || count > definition.MaxStockSize)
                throw new ArgumentException();

            _definition = definition;
            _count = count;
        }

        internal int AppendStock(int count) => SetStock(_count + count);
        internal int RemoveStock(int count) => SetStock(_count - count);
        internal int SetStock(int count)
        {
            _count = Mathf.Clamp(count, _definition.MinStockSize, _definition.MaxStockSize);
            return count - _count;
        }
        internal ItemInstance Copy() => new(Definition, Count);
    }
}
