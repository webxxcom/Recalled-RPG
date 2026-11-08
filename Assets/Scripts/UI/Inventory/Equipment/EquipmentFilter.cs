using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    internal sealed class EquipmentFilter : MonoBehaviour
    {
        [SerializeField] SlotType _slot;
        [SerializeField] EquipmentItemsView _view;

        public void Filter(bool isOn)
        {
            if (isOn) _view.Filter(_slot);
        }
    }
}
