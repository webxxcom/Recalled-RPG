using Recalled.Core;
using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using Recalled.UI.Inventory;
using System.Linq;
using UnityEngine;

namespace Recalled.UI
{
    internal sealed class EquippedSlotsView : UiView<Equipment>
    {
        [SerializeField] EquipmentAnchor _equipmentAnchor;

        InventoryCell[] _cells;
        protected override RuntimeAnchor<Equipment> Anchor => _equipmentAnchor;
        void Awake() => _cells = GetComponentsInChildren<InventoryCell>();

        protected override void Refresh()
        {
            var equipment = _equipmentAnchor.Value;

            int i = 0;
            foreach (var equippedItem in equipment.Equipped.OrderBy(i => i.Equippable.Slot))
                _cells[i++].ShowItem(equippedItem.Item, false);

            for (; i < _cells.Length; i++)
                _cells[i].ShowEmpty();
        }
    }
}
