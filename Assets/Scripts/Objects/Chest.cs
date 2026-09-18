using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Lootable))]
public class Chest : SingleTimeInteractableObject
{
    [SerializeField] ItemDefinition _requiredKey;
    [SerializeField] InventorySO _inventory;
    [SerializeField] PopupWorldText _rejectItemText;

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

            if (_lootable.LootItem()) _inventory.GeneralLoadout.Remove(_requiredKey);
            enabled = false;
        }
    }

    public override bool PlayerCanInteract()
    {
        return enabled && (_requiredKey == null || _inventory.GeneralLoadout.Has(_requiredKey));
    }
}
