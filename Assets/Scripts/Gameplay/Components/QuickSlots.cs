using Recalled.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class QuickSlots : IReadOnlyList<ItemInstance>
    {
        public class Slot
        {
            public readonly ItemDefinition Definition;
            public int Count;

            public Slot(ItemDefinition definition, int count)
            {
                Definition = definition;
                Count = count;
            }
        }

        readonly Bag _inventory;
        readonly Slot[] _slots;
        public int Count => 4;
        public ItemInstance this[int index] => _slots[index]?.Definition.CreateInstance(_slots[index].Count);

        public QuickSlots(Bag bag)
        {
            _slots = new Slot[Count];
            _inventory = bag;

            _inventory.SlotsChanged += UpdateAll;
            UpdateAll();
        }

        void UpdateAll()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == null)
                    continue;

                var slot = _slots[i];

                _slots[i].Count =
                    _inventory.Items
                        .Where(i => i?.Definition == slot.Definition)
                        .Sum(i => i.Count);
            }
        }

        public void Put(int i, ItemDefinition def)
        {
            if (def == null) throw new ArgumentNullException();

            _slots[i] = new(def, _inventory.Items.Where(i => i?.Definition == def).Sum(i => i.Count));
        }
        public void Remove(int i)
        {
            _slots[i] = default;
        }

        public void Use(int i, GameObject consumer)
        {
            if (_slots[i] == null) return;

            if (!_slots[i].Definition.TryGetModule<Consumable>(out var consumeModule))
                return;

            if (_inventory.Take(_slots[i].Definition, 1))
            {
                _slots[i].Count--;
                consumeModule.Consume(consumer);
            }
        }

        public IEnumerator<ItemInstance> GetEnumerator()
            => _slots.Select(s => s.Definition.CreateInstance(s.Count)).AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
