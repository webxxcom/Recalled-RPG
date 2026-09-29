using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTable
{
    [System.Serializable]
    public class LootItem
    {
        [SerializeField] ItemDefinition _item;
        [SerializeField] int _weight;

        public int GetWeight() => _weight;
        public ItemDefinition GetItemDefinition() => _item;
    }

    [SerializeField] List<LootItem> _loots;

    int TotalWeight
    {
        get
        {
            int total = 0;
            foreach (var item in _loots)
            {
                total += item.GetWeight();
            }
            return total;
        }
    }

    public ItemDefinition GetItem()
    {
        float expectedWeight = Random.Range(0, TotalWeight);

        int totalWeight = 0;
        foreach (var item in _loots)
        {
            totalWeight += item.GetWeight();
            if (totalWeight >= expectedWeight)
                return item.GetItemDefinition();
        }
        Debug.LogError("Unexpected error while getting Loot");
        return null;
    }
}
