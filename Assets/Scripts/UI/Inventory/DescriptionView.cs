using Recalled.Core;
using Recalled.Systems.Inventory;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    internal sealed class DescriptionView : MonoBehaviour
    {
        [SerializeField] InventoryCell _cell;
        [SerializeField] TMP_Text _header;
        [SerializeField] StatsView _statsView;
        [SerializeField] TMP_Text _description;

        [Header("Reads")]
        [SerializeField] GameObjectRuntimeVariable _currentSelected;

        private void OnEnable() => _currentSelected.Subscribe(OnCurrentSelected);
        private void OnDisable() => _currentSelected.Unsubscribe(OnCurrentSelected);

        void Show(ItemInstance instance)
        {
            _cell.ShowItem(instance, false);
            _statsView.UpdateStats(instance.Definition.GetModules<IExtraDescription>().SelectMany(d => d.ExtraDescription).ToArray());
            _header.text = instance.Definition.Name;
            _description.text = instance.Definition.Description;
        }

        void Hide()
        {
            if (_cell.Item != null) _cell.Dim();
            else _cell.ShowEmpty();
        }

        void OnCurrentSelected(GameObject game)
        {
            if (game != null && game.TryGetComponent<IItemHolder>(out var itemHolder))
                Show(itemHolder.Item);
            else Hide();
        }
    }
}
