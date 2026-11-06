using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.Gameplay.Inventory
{
    internal sealed class InventoryHolder : MonoBehaviour
    {
        [SerializeField] Loadout _loadout;
        [SerializeField] InventoryAnchor _inventoryAnchor;

        Bag _reference;
        public Bag Reference
        {
            get
            {
                _reference ??= new(_loadout.Capacity, _loadout.Items);

                return _reference;
            }
        }
        private void Awake() => _inventoryAnchor.Provide(Reference);
        private void OnDestroy() => _inventoryAnchor.Withdraw(Reference);
    }
}
