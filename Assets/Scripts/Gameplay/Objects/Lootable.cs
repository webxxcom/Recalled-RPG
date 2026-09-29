using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    public class Lootable : MonoBehaviour
    {
        [SerializeField] InventorySO _inventory;
        [SerializeField] LootTable _lootTable;

        public UnityEvent Looted;

        public bool LootItem()
        {
            ItemInstance item = _lootTable.GetItem().CreateInstance();

            if (_inventory.GeneralItems.Add(item))
            {
                Looted.Invoke();
                return true;
            }
            return false;
        }
    }
}
