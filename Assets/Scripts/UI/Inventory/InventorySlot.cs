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

        public override void SetItem(ItemInstance instance)
        {
            base.SetItem(instance);

            _selectable.enabled = true;
            if (instance.Definition.IsStackable)
            {
                _countText.enabled = true;
                _countText.text = instance.Count.ToString();
            }
            else
                _countText.enabled = false;
        }

        public override void RemoveItem()
        {
            _selectable.enabled = false;
            _countText.enabled = false;

            base.RemoveItem();
        }
    }
}
