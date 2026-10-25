using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    sealed class InventoryFilter : MonoBehaviour
    {
        [SerializeField] ItemCategory _category;
        [SerializeField] InventorySlotsView _view;

        public void MakeFilter(bool isOn)
        {
            if (isOn) _view.Filter(_category);
            else _view.Filter(ItemCategory.Any);
        }
    }
}
