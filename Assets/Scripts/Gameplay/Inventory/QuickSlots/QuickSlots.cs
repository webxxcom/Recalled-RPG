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
                    res[i] = this[i];
                return res;
            }
        }
        public SlotView this[int index]
        {
            get
            {
                var def = _storedDefinitions[index];

                return def != null
                    ? new(def, _inventoryView.TotalCountOf(def))
                    : default;
            }
        }

        public QuickSlots(ItemDefinition[] loadout, IBagView inventoryView)
        {
            _inventoryView = inventoryView;
            for (int i = 0; i < _storedDefinitions.Length && i < loadout.Length; i++)
                _storedDefinitions[i] = loadout[i];

            _inventoryView.Changed += () => Changed?.Invoke();
        }
    }
}
