using UnityEngine;

public interface ILootable
{
    public Transform Transform { get; }
    public LootTable LootTable { get; }
}
