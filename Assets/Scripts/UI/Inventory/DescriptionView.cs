using Recalled.Core;
using Recalled.Systems.Inventory;
using TMPro;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    internal sealed class DescriptionView : MonoBehaviour
    {
        [SerializeField] InventoryCell _cell;
        [SerializeField] TMP_Text _header;
        [SerializeField] TMP_Text _extra;
        [SerializeField] TMP_Text _description;

        [Header("Reads")]
        [SerializeField] GameObjectRuntimeVariable _currentSelected;

        private void OnEnable() => _currentSelected.Subscribe(OnCurrentSelected);
        private void OnDisable() => _currentSelected.Unsubscribe(OnCurrentSelected);

        void Show(ItemInstance instance)
        {
            _cell.ShowItem(instance, false);
            _header.text = instance.Definition.Name;
            _extra.text = instance.Definition.Extra;
            _description.text = instance.Definition.Description;
        }

        void Hide()
        {
            if (_cell.ItemInstance != null) _cell.Dim();
            else _cell.ShowEmpty();
        }

        void OnCurrentSelected(GameObject game)
        {
            if (game != null && game.TryGetComponent<InventorySlot>(out var slot))
                Show(slot.ItemInstance);
            else Hide();
        }
    }
}
