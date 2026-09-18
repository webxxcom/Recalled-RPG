using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Player Inventory")]
public class InventorySO : ScriptableObject
{
    [SerializeField] InventoryItemCollectionSO _generalItems;
    [SerializeField] InventoryItemCollectionSO _quickItems;

    public Sword Sword { get; set; }
    public Armor Armor { get; set; }
    public Boots Boots { get; set; }

    public InventoryItemCollectionSO GeneralLoadout => _generalItems;
    public InventoryItemCollectionSO QuickLoadout => _quickItems;
}
