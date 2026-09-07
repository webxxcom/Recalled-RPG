using UnityEngine;

[RequireComponent(typeof(Lootable))]
public class Chest : SingleTimeInteractableObject
{
    [SerializeField] ItemDefinition _requiredKey;
    [SerializeField] InventorySO _inventory;

    public override void Interact()
    {
        if (PlayerCanInteract())
            Open();
    }

    void Open()
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
