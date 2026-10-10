using Recalled.Systems.Inventory;
using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    public class Lootable : MonoBehaviour
    {
        [SerializeField] LootTable _lootTable;

        public event Action Looted;

        public ItemInstance LootItem()
        {
            Looted?.Invoke();
            return _lootTable.GetItem();
        }
    }
}
