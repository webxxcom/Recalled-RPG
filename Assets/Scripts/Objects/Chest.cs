using UnityEngine;

public class Chest : SingleTimeInteractableObject, ILootable
{
    [SerializeField] ItemDefinition _requiredKey;
    [SerializeField] LootTable _lootTable;
    [SerializeField] InventorySO _inventory;
    [SerializeField] PopupWorldText _rejectItemText;

    public override void Interact()
    {
        if (PlayerCanInteract())
        {
            IsInteracted = true;

            LootItem(_inventory, _lootTable.GetItem().CreateInstance());
            enabled = false;
        }
    }

    public override bool PlayerCanInteract()
    {
        return enabled && (_requiredKey == null || _inventory.Contains(_requiredKey));
    }

    public void LootItem(InventorySO inventory, ItemInstance item)
    {
        if (inventory.AddItem(item)) inventory.Remove(_requiredKey);
        else RejectItem(item);
    }

    void RejectItem(ItemInstance item)
    {
        PopupWorldText pwt = Instantiate(_rejectItemText, transform.position, Quaternion.identity);
        pwt.Init($"'{item.Definition.Name}'\nrejected..");
    }
}
