using Recalled.Core;
using Recalled.Systems.Inventory;
using TMPro;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    public class DescriptionView : MonoBehaviour
    {
        [SerializeField] InventoryCell _cell;
        [SerializeField] TextMeshProUGUI _header;
        [SerializeField] TextMeshProUGUI _extra;
        [SerializeField] TextMeshProUGUI _description;

        [Header("Reads")]
        [SerializeField] GameObjectRuntimeVariable _currentSelected;

        private void OnEnable() => _currentSelected.Subscribe(OnCellSelected);
        private void OnDisable() => _currentSelected.Unsubscribe(OnCellSelected);

        void Show(ItemInstance instance)
        {
            _cell.ShowItem(instance, false);
            _header.text = instance.Definition.Name;
            _description.text = instance.Description;
        }

        void Hide()
        {
            _cell.ShowEmpty();
            _header.text = "";
            _description.text = "";
        }

        void OnCellSelected(GameObject game)
        {
            if (game != null && game.TryGetComponent<InventorySlot>(out var slot))
                Show(slot.ItemInstance);
            else Hide();
        }
    }
}
