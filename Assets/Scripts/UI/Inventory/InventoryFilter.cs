using Recalled.Systems.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI.Inventory
{
    public sealed class InventoryFilter : MonoBehaviour
    {
        [SerializeField] ItemCategory _category;
        [SerializeField] InventorySlotsView _view;
        [SerializeField] Sprite _pressedSprite;
        [SerializeField] Sprite _releasedSprite;
        [SerializeField] Image _graphic;

        private void OnEnable() => MakeFilter(false);
        public void MakeFilter(bool isOn)
        {
            if (isOn)
            {
                _graphic.sprite = _pressedSprite;
                _view.Filter(_category);
            }
            else
            {
                _graphic.sprite = _releasedSprite;
                _view.Filter(ItemCategory.Any);
            }
        }
    }
}
