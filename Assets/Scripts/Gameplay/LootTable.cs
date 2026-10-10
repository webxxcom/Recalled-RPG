using Recalled.Systems.Inventory;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.Gameplay
{
    [System.Serializable]
    public class LootTable
    {
        [System.Serializable]
        class LootItem
        {
            [SerializeField] ItemDefinition _item;
            [SerializeField] int _count;
            [SerializeField] int _weight;

            public ItemDefinition ItemDefinition => _item;
            public int Count => _count;
            public int Weight => _weight;
        }

        [SerializeField] List<LootItem> _loots;

        int TotalWeight => _loots.Sum(i => i.Weight);

        public ItemInstance GetItem()
        {
            float expectedWeight = Random.Range(0, TotalWeight);

            int totalWeight = 0;
            foreach (var loot in _loots)
            {
                totalWeight += loot.Weight;
                if (totalWeight >= expectedWeight)
                    return loot.ItemDefinition.CreateInstance(loot.Count);
            }
            Debug.LogWarning("Loot table has no entries");
            return null;
        }
    }
}
