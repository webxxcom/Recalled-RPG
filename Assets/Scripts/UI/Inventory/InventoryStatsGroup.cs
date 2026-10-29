using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using System.Linq;
using UnityEngine;

namespace Recalled.UI
{
    sealed class InventoryStatsGroup : MonoBehaviour
    {
        [SerializeField] InventoryAnchor _inventoryAnchor;

        InventoryStatsHolder[] _statsHolders;
        IBagView _inventoryView;
        
        private void Awake() => _statsHolders = GetComponentsInChildren<InventoryStatsHolder>();
        private void OnEnable()
        {
            _inventoryAnchor.Provided += OnAnchorProvided;

            if (_inventoryAnchor.IsSet) OnAnchorProvided(_inventoryAnchor.Value);
        }
        private void OnDisable() => _inventoryAnchor.Provided -= OnAnchorProvided;

        void OnAnchorProvided(IBagView view)
        {
            _inventoryView = view;

            _inventoryView.Changed += OnViewChanged;
            OnViewChanged();
        }

        void OnViewChanged()
        {
            foreach (var statHolder in _statsHolders)
            {
                var n = _inventoryView.Where(i => i != null && i.Definition.Category == statHolder.Group).Count();

                statHolder.Show(n, _inventoryView.Count);
            }
        }
    }
}
