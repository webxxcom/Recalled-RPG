using UnityEngine;

public class Chest : SingleTimeInteractableObject, ILootable
{
    [SerializeField] ItemDefinition _requiredKey;
    [SerializeField] LootTable _lootTable;
    [SerializeField] InventorySO _inventory;

    public Transform Transform => transform;
    public LootTable LootTable => _lootTable;

    public override void Interact()
    {
        if (PlayerCanInteract())
            Open();
    }

    void Open()
    {
        IsInteracted = true;

        _inventory.Remove(_requiredKey);
        _inventory.Add(this);
        enabled = false;
    }

    public override bool PlayerCanInteract()
    {
        return !IsInteracted && (_requiredKey == null || _inventory.Contains(_requiredKey));
    }
}
