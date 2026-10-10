using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Recalled.Systems.Inventory
{
    public sealed class ItemSlotsArray : IReadOnlyList<ItemSlotsArray.Slot>
    {
        public readonly struct Slot
        {
            public readonly ItemInstance Item;
            public readonly bool IsEmpty => Item == null;

            internal Slot(ItemInstance instance)
            {
                Item = instance ?? throw new ArgumentNullException();
            }
        }

        readonly Slot[] _slots;
        public int Count => _slots.Length;
        public Slot this[int index] => _slots[index];

        public event Action SlotsChanged;

        public ItemSlotsArray(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException();

            _slots = new Slot[capacity];
        }

        public ItemSlotsArray(int capacity, ItemInstance[] loadout)
        {
            if (loadout == null)
                throw new ArgumentNullException();
            if (capacity <= 0 || loadout.Length > capacity)
                throw new ArgumentException();

            _slots = new Slot[capacity];
            for (int i = 0; i < loadout.Length; i++)
                _slots[i] = new(loadout[i].Copy());
        }

        public int Add(ItemDefinition definition, int count)
        {
            int remainder = count;

            if (definition == null)
                throw new ArgumentNullException();
            if (count <= 0)
                throw new ArgumentException();

            if (definition.IsStackable)
            {
                remainder = AddStockable(definition, remainder);
                if (remainder == 0)
                {
                    SlotsChanged?.Invoke();
                    return 0;
                }
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (!_slots[i].IsEmpty)
                    continue;

                var instance = definition.CreateInstance();
                remainder = instance.SetStock(remainder);

                _slots[i] = new(instance);
                if (remainder <= 0)
                    break;
            }

            if (remainder != count) SlotsChanged?.Invoke();
            return remainder;
        }

        int AddStockable(ItemDefinition definition, int count)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].IsEmpty || _slots[i].Item.Definition != definition)
                    continue;

                count = _slots[i].Item.AppendStock(count);
                if (count <= 0) break;
            }
            return count;
        }

        public bool Add(ItemInstance instance)
        {
            if (instance == null || _slots.Any(s => s.Item == instance))
                throw new ArgumentNullException();

            int initCount = instance.Count;
            if (instance.Definition.IsStackable)
            {
                var remainder = AddStockable(instance.Definition, instance.Count);
                if (remainder == 0)
                {
                    SlotsChanged?.Invoke();
                    return true;
                }
                instance.SetStock(remainder);
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (!_slots[i].IsEmpty)
                    continue;

                _slots[i] = new(instance);
                SlotsChanged?.Invoke();
                return true;
            }

            if (instance.Count != initCount)
                SlotsChanged?.Invoke();

            return false;
        }

        /// <summary>Remove this exact item instance from the inventory</summary>
        public bool Remove(ItemInstance itemInstance)
        {
            if (itemInstance == null)
                throw new ArgumentNullException();

            for (int i = 0; i < _slots.Length; ++i)
            {
                if (_slots[i].Item != itemInstance)
                    continue;

                _slots[i] = default;
                SlotsChanged?.Invoke();
                return true;
            }

            return false;
        }
        public bool Remove(ItemDefinition itemDefinition)
            => Take(itemDefinition, 1);

        bool TakeRecursively(ItemDefinition definition, int count, int i)
        {
            for (; i < _slots.Length; ++i)
            {
                if (_slots[i].IsEmpty)
                    continue;

                var item = _slots[i].Item;
                if (item.Definition != definition)
                    continue;

                int newCount = item.Count - count;
                if (newCount == 0)
                    _slots[i] = default;
                if (newCount < 0)
                {
                    if (!TakeRecursively(definition, -newCount, i + 1))
                        return false;

                    _slots[i] = default;
                }
                else
                    item.SetStock(newCount);

                return true;
            }

            return false;
        }

        public bool Take(ItemDefinition definition, int count)
        {
            if (definition == null)
                throw new ArgumentNullException();
            if (count <= 0)
                throw new ArgumentException();

            if (TakeRecursively(definition, count, 0))
            {
                SlotsChanged?.Invoke();
                return true;
            }
            return false;
        }

        public IEnumerator<Slot> GetEnumerator()
            => _slots.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
