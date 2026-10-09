using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.Gameplay
{
    public class InventoryHolder : MonoBehaviour
    {
        [SerializeField] Loadout _loadout;
        [SerializeField] InventoryVariable _inventoryVariable;

        ItemSlotsArray _inventory;
        public ItemSlotsArray Inventory => _inventory;

        private void Awake()
        {
            _inventory = new(_loadout.Capacity, _loadout.Items);
        }

        private void Start()
        {
            _inventoryVariable.Value = _inventory;
        }
    }
}
