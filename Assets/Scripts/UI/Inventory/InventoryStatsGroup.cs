using Recalled.Core;
using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using System.Linq;
using UnityEngine;

namespace Recalled.UI
{
    // TODO this view implements the UiView but does not use the prefab which actually leads to a thing
    //  that a view must not be hardcoded to openClosed as in which code, it should create the object rather
    //  than collect them
    sealed class InventoryStatsGroup : UiView<IBagView>
    {
        [SerializeField] InventoryAnchor _inventoryAnchor;

        InventoryStatsHolder[] _statsHolders;
        protected override RuntimeAnchor<IBagView> Anchor => _inventoryAnchor;
        private void Awake() => _statsHolders = GetComponentsInChildren<InventoryStatsHolder>();

        protected override void Refresh()
        {
            var inventory = _inventoryAnchor.Value;
            foreach (var statHolder in _statsHolders)
            {
                var n = inventory.Where(i => i != null && i.Definition.IsInCategory(statHolder.Group)).Count();

                statHolder.Show(n, inventory.Count);
            }
        }
    }
}
