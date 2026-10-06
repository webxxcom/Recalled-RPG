using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    public class Lootable : MonoBehaviour
    {
        public readonly struct LootedItem
        {
            public readonly ItemDefinition Definition;
            public readonly int Count;

            public LootedItem(ItemDefinition definition, int count)
            {
                Definition = definition;
                Count = count;
            }
        }

        [SerializeField] LootTable _lootTable;

        public event Action Looted;

        public LootedItem LootItem()
        {
            Looted?.Invoke();
            return new(_lootTable.GetItem(), 1);
        }
    }
}
