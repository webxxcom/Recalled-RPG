using Recalled.Systems.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.UI.Inventory
{
    sealed class InventorySlot : InventoryCell
    {
        [SerializeField] TMP_Text _countText;
        [SerializeField] UnityEvent<bool> _itemPresented;

        public override void ShowItem(ItemInstance instance, bool showDimmed)
        {
            base.ShowItem(instance, showDimmed);

            if (showDimmed)
            {
                _countText.color = _dimColor;
                _itemPresented.Invoke(false);
            }
            else
            {
                _itemPresented.Invoke(true);
                _countText.color = Color.white;
            }

            if (instance.Definition.IsStackable)
            {
                _countText.enabled = true;
                _countText.text = instance.Count.ToString();
            }
            else
                _countText.enabled = false;

        }

        public override void ShowEmpty()
        {
            _countText.enabled = false;

            base.ShowEmpty();
            _itemPresented.Invoke(false);
        }
    }
}
