using Recalled.Systems.Inventory;
using TMPro;
using UnityEngine;

namespace Recalled.UI
{
    public class InventoryStatsHolder : MonoBehaviour
    {
        [SerializeField] TMP_Text _tmpText;
        [SerializeField] ItemCategory _group;

        public ItemCategory Group => _group;

        public void Show(int current, int max)
        {
            _tmpText.text = $"{current}/{max}";
        }
    }
}
