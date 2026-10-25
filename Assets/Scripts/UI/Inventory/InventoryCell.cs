using Recalled.Systems.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI.Inventory
{
    class InventoryCell : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] protected Color _dimColor;

        public ItemInstance ItemInstance { get; private set; }

        private void OnEnable()
        {
            ShowEmpty();
        }

        public virtual void ShowItem(ItemInstance instance, bool showDimmed)
        {
            ItemInstance = instance;
            _icon.sprite = ItemInstance.Definition.Icon;
            _icon.preserveAspect = true;
            _icon.enabled = true;
            _icon.color = showDimmed ? _dimColor : Color.white;
        }

        public virtual void ShowEmpty()
        {
            ItemInstance = null;
            _icon.enabled = false;
        }
    }
}
