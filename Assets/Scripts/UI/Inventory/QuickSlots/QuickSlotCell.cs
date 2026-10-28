using Recalled.Systems.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI.Inventory
{
    sealed class QuickSlotCell : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] Color _dimColor;
        [SerializeField] TMP_Text _text;

        public void Show(ItemDefinition definition, int count)
        {
            if (definition == null)
                return;

            _icon.enabled = true;
            _icon.sprite = definition.Icon;
            _text.text = count.ToString();

            _icon.color = count > 0 ? Color.white : _dimColor;
            _text.enabled = count > 0;
        }

        public void Remove()
        {
            _icon.enabled = false;
            _text.enabled = false;
        }
    }
}
