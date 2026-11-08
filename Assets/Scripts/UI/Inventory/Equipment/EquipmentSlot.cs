using Recalled.Systems.Inventory;
using TMPro;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    internal sealed class EquipmentSlot : MonoBehaviour, IItemHolder
    {
        static readonly string EQUIPPED_STR = "Equipped";
        static readonly string UNEQUIPPED_STR = "";

        [SerializeField] TMP_Text _nameTmp;
        [SerializeField] TMP_Text _statusTmp;

        public ItemInstance Item { get; private set; }
        public ItemInstance Displayed => Item;

        public void ShowItem(Equipment.EquippableItem equippableItem)
        {
            Item = equippableItem.Item;

            _nameTmp.text = equippableItem.Item.Definition.Name;
            _statusTmp.text = equippableItem.IsEquipped ? EQUIPPED_STR : UNEQUIPPED_STR;
        }
    }
}
