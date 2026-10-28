using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.Gameplay.Inventory
{
    public sealed class InventoryHolder : MonoBehaviour
    {
        [SerializeField] Loadout _loadout;
        [SerializeField] InventoryAnchor _inventoryAnchor;

        public Bag Inventory { get; private set; }

        private void Awake()
        {
            Inventory = new(_loadout.Capacity, _loadout.Items);

            _inventoryAnchor.Provide(Inventory);
        }
        private void OnDestroy() => _inventoryAnchor.Withdraw(Inventory);
    }
}
