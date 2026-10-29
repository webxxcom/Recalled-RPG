using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.Gameplay.Inventory
{
    internal sealed class InventoryHolder : MonoBehaviour
    {
        [SerializeField] Loadout _loadout;
        [SerializeField] InventoryAnchor _inventoryAnchor;

        public Bag Reference { get; private set; }

        private void Awake()
        {
            Reference = new(_loadout.Capacity, _loadout.Items);

            _inventoryAnchor.Provide(Reference);
        }
        private void OnDestroy() => _inventoryAnchor.Withdraw(Reference);
    }
}
