using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LootTable
{
    [System.Serializable]
    public class LootItem
    {
        [SerializeField] ItemDefinition _item;
        [SerializeField] int _weight;

        public int Weight => _weight;
        public ItemDefinition ItemDefinition => _item;
    }

    [SerializeField] List<LootItem> _loots;

    int TotalWeight => _loots.Sum(i => i.Weight);

    public ItemDefinition GetItem()
    {
        float expectedWeight = Random.Range(0, TotalWeight);

        int totalWeight = 0;
        foreach (var loot in _loots)
        {
            totalWeight += loot.Weight;
            if (totalWeight >= expectedWeight)
                return loot.ItemDefinition;
        }
        Debug.LogWarning("Loot table has no entries");
        return null;
    }
}
