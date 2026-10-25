using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class InventoryHolder : MonoBehaviour
    {
        [SerializeField] Loadout _loadout;
        [SerializeField] InventoryVariable _inventoryVariable;

        public Bag Inventory { get; private set; }

        private void Awake()
        {
            Inventory = new(_loadout.Capacity, _loadout.Items);
        }

        private void Start()
        {
            _inventoryVariable.Value = Inventory;
        }
    }
}
