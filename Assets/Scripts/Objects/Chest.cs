using UnityEngine;

[RequireComponent(typeof(Lootable))]
public class Chest : SingleTimeInteractableObject
{
    [SerializeField] ItemDefinition _requiredKey;
    [SerializeField] InventorySO _inventory;

    Lootable _lootable;

    protected override void Awake()
    {
        base.Awake();

        _lootable = GetComponent<Lootable>();
    }

    public override void Interact()
    {
        if (PlayerCanInteract())
        {
            IsInteracted = true;

            if (_lootable.LootItem()) _inventory.Remove(_requiredKey);
            enabled = false;
        }
        
    }

    public override bool PlayerCanInteract()
    {
        return enabled && (_requiredKey == null || _inventory.Contains(_requiredKey));
    }
}
