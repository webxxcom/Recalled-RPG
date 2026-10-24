using Recalled.Systems.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI.Inventory
{
    class InventoryCell : MonoBehaviour
    {
        [SerializeField] Image _icon;

        public ItemInstance ItemInstance { get; private set; }

        public virtual void SetItem(ItemInstance instance)
        {
            ItemInstance = instance;
            _icon.sprite = ItemInstance.Definition.Icon;
            _icon.preserveAspect = true;
            _icon.enabled = true;
        }

        public virtual void RemoveItem()
        {
            ItemInstance = null;
            _icon.enabled = false;
        }
    }
}
