using Recalled.Gameplay.Inventory;
using UnityEngine;

namespace Recalled.UI
{
    internal sealed class EquipmentFilter : MonoBehaviour
    {
        [SerializeField] SlotType _slot;
        [SerializeField] InventoryItemsView _view;

        public void Filter(bool isOn)
        {
            if (isOn) _view.Filter(_slot);
        }
    }
}
