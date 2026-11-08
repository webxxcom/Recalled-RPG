using System;
using System.Collections.Generic;
using System.Linq;

namespace Recalled.Systems.Inventory
{
    public sealed class Equipment : IView
    {
        public readonly struct EquippableItem
        {
            public EquippableItem(ItemInstance item, EquippableModule equippable, bool isEquipped)
            {
                Item = item;
                Equippable = equippable;
                IsEquipped = isEquipped;
            }

            public ItemInstance Item { get; }
            public EquippableModule Equippable { get; }
            public bool IsEquipped { get; }
        }

        readonly IBagView _bag;
        readonly Dictionary<SlotType, ItemInstance> _slots = new();
        public event Action Changed;
        public IReadOnlyList<EquippableItem> EquippableItems
        {
            get
            {
                List<EquippableItem> res = new();
                foreach (var item in _bag)
                {
                    if (item == null)
                        continue;

                    var equippable = item.Equippable(this);
                    if (equippable.HasValue)
                        res.Add(equippable.Value);
                }
                return res;
            }
        }
        public IReadOnlyList<EquippableItem> Equipped => EquippableItems.Where(ei => ei.IsEquipped).ToArray();
        internal IReadOnlyDictionary<SlotType, ItemInstance> Slots => _slots;

        public Equipment(IBagView bag)
        {
            _bag = bag;

            _bag.Changed += EnsureValidState;
        }

        void EnsureValidState()
        {
            List<SlotType> toRemove = new();

            foreach (var slot in _slots)
                if (!_bag.Contains(slot.Value))
                    toRemove.Add(slot.Key);

            foreach (var slotType in toRemove)
                _slots.Remove(slotType);
            Changed?.Invoke();
        }

        public bool Equip(ItemInstance item)
        {
            if (!_bag.Contains(item) || !item.Definition.TryGetModule<EquippableModule>(out var equippable))
                return false;

            var slot = equippable.Slot;
            _slots[slot] = item ?? throw new ArgumentNullException();
            Changed?.Invoke();
            return true;
        }

        public void Unequip(ItemInstance item)
        {
            var equippableItem = item.Equippable(this);
            if (!equippableItem.HasValue)
                return;

            var slot = equippableItem.Value.Equippable.Slot;
            if (_slots.Remove(slot))
                Changed?.Invoke();
        }
    }
}
