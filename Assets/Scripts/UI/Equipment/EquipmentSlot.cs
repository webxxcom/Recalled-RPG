using Recalled.Systems.Inventory;
using Recalled.UI.Inventory;
using TMPro;
using UnityEngine;

namespace Recalled.UI
{
    internal sealed class EquipmentSlot : MonoBehaviour, IItemHolder
    {
        static readonly string EQUIPPED_STR = "Equipped";
        static readonly string UNEQUIPPED_STR = "";

        [SerializeField] TMP_Text _nameTmp;
        [SerializeField] TMP_Text _statusTmp;

        public ItemInstance Item { get; private set; }
        public ItemInstance Displayed => Item;

        public void ShowItem(Bag.Slot itemSlot)
        {
            Item = itemSlot.Item;

            _nameTmp.text = itemSlot.Item.Definition.Name;
            _statusTmp.text = itemSlot.IsEquppied ? EQUIPPED_STR : UNEQUIPPED_STR;
        }
    }
}
