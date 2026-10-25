using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class InventoryHolder : MonoBehaviour
    {
        [SerializeField] Loadout _loadout;
        [SerializeField] InventoryVariable _inventoryVariable;
        [SerializeField] InventoryVariable _quickSlotsVariable;

        public ItemSlotsArray Inventory { get; private set; }
        public ItemSlotsArray QuickSlots { get; private set; }

        private void Awake()
        {
            Inventory = new(_loadout.Capacity, _loadout.Items);
            QuickSlots = new(4);
        }

        private void Start()
        {
            _inventoryVariable.Value = Inventory;
            _quickSlotsVariable.Value = QuickSlots;
        }
    }
}
