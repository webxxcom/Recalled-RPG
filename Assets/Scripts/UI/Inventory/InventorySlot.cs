using Recalled.Systems.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI.Inventory
{
    [RequireComponent(typeof(Selectable))]
    sealed class InventorySlot : InventoryCell
    {
        [SerializeField] TMP_Text _countText;
        Selectable _selectable;

        private void Awake() => _selectable = GetComponent<Selectable>();

        public override void ShowItem(ItemInstance instance, bool showDimmed)
        {
            base.ShowItem(instance, showDimmed);

            if (showDimmed)
            {
                _selectable.enabled = false;
                _countText.color = _dimColor;
            }
            else
            {
                _selectable.enabled = true;
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
            _selectable.enabled = false;
            _countText.enabled = false;

            base.ShowEmpty();
        }
    }
}
