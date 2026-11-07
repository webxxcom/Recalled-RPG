using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.UI
{
    internal abstract class InventoryItemsView : MonoBehaviour
    {
        [SerializeField] protected InventoryAnchor _inventoryAnchor;
        [SerializeField] protected PrefabPopulator _populator;

        protected bool _isDirty;

        void OnEnable()
        {
            _inventoryAnchor.Provided += OnInventoryProvided;

            if (_inventoryAnchor.IsSet) OnInventoryProvided(_inventoryAnchor.Value);
        }
        void OnDisable() => _inventoryAnchor.Provided -= OnInventoryProvided;

        void OnInventoryProvided(IBagView inventory)
        {
            inventory.Changed += () => _isDirty = true;
            _isDirty = true;
        }

        public abstract void RefreshView();

        private void LateUpdate()
        {
            if (_isDirty)
            {
                _isDirty = false;

                RefreshView();
            }
        }
    }
}
