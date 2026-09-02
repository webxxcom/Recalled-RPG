using UnityEngine;

public class Chest : SingleTimeInteractableObject
{
    [SerializeField] ItemDefinition _requiredKey;
    [SerializeField] LootTable _lootTable;
    [SerializeField] InventorySO _inventory;

    public override void Interact()
    {
        if (PlayerCanInteract())
            Open();
    }

    void Open()
    {
        IsInteracted = true;

        _inventory.Remove(_requiredKey);
        _inventory.Add(_lootTable.GetItem());
        enabled = false;
    }

    public override bool PlayerCanInteract()
    {
        return !IsInteracted && (_requiredKey == null || _inventory.Contains(_requiredKey));
    }
}
