using Recalled.Systems.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI.Inventory
{
    class InventoryCell : MonoBehaviour, IItemHolder
    {
        [SerializeField] Image _icon;
        [SerializeField] protected Color _dimColor;

        public ItemInstance Item { get; private set; }

        private void OnEnable() => ShowEmpty();

        public virtual void ShowItem(ItemInstance instance, bool showDimmed)
        {
            Item = instance;
            _icon.sprite = Item.Definition.Icon;
            _icon.preserveAspect = true;
            _icon.enabled = true;
            _icon.color = showDimmed ? _dimColor : Color.white;
        }

        public void Dim() => _icon.color = _dimColor;

        public virtual void ShowEmpty()
        {
            Item = null;
            _icon.enabled = false;
        }
    }
}
