using Recalled.Systems.Inventory;
using System;
using System.Linq;

namespace Recalled.Gameplay.Inventory
{
    public sealed class QuickSlots : IQuickSlotsView
    {
        public readonly struct SlotView : IEquatable<SlotView>
        {
            public ItemDefinition Definition { get; }
            public int Count { get; }

            public SlotView(ItemDefinition definition, int count)
            {
                Definition = definition;
                Count = count;
            }

            public bool Equals(SlotView other) => other.Definition == Definition && other.Count == Count;
        }
        readonly ItemDefinition[] _storedDefinitions = new ItemDefinition[4];
        int Count => _storedDefinitions.Length;
        readonly IBagView _inventoryView;
        public event Action Changed;

        public SlotView[] Slots
        {
            get
            {
                var res = new SlotView[Count];
                for (int i = 0; i < res.Length; i++)
                {
                    var def = _storedDefinitions[i];

                    res[i] = def != null ? new(def, _inventoryView.TotalCountOf(def)) : default;
                }
                return res;
            }
        }
        public ItemDefinition this[int index] => _storedDefinitions[index];

        public QuickSlots(ItemDefinition[] loadout, IBagView inventoryView)
        {
            _inventoryView = inventoryView;
            for (int i = 0; i < _storedDefinitions.Length && i < loadout.Length; i++)
                _storedDefinitions[i] = loadout[i];

            _inventoryView.Changed += () => Changed?.Invoke();
        }

        public void RequestDefinitionForSlot(ItemDefinition def, int i)
        {
            if (i < 0 || i >= _storedDefinitions.Length)
                throw new ArgumentException();

            if (def != _storedDefinitions[i] && _storedDefinitions.Any(d => d == def))
                return; // Do not put another same def into different slot

            if (!def.IsInCategory(ItemCategory.Consumable))
                return; // not consumable can't be put there

            if (def == _storedDefinitions[i]) _storedDefinitions[i] = default;
            else _storedDefinitions[i] = def;
            Changed?.Invoke();
        }
    }
}
