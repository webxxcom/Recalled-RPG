using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Lootable))]
    public sealed class Chest : Interactable
    {
        [SerializeField] ItemDefinition _requiredKey;

        Lootable _lootable;

        public enum States
        {
            Opened,
            Rejected,
            Closed
        }
        public States State { get; private set; } = States.Closed;

        public override bool CanBeInteracted
            => enabled && (_requiredKey == null);// || _inventory.GeneralItems.Has(_requiredKey));

        protected override void Awake()
        {
            base.Awake();

            _lootable = GetComponent<Lootable>();
        }

        public override void Interact(InteractionProcessor interactor)
        {
            if (!CanBeInteracted)
                return;

            var looted = _lootable.LootItem();
            var inventory = interactor.GetComponentInParent<InventoryHolder>().Inventory;
            if (_requiredKey == null || inventory.Take(_requiredKey, 1))
            {
                // Successful interaction only if item can be added
                inventory.Add(looted);
                State = States.Opened;
                enabled = false;
            }
            else State = States.Rejected;

            //Even if interaction is not successful reactors check the Chest.State
            base.Interact(interactor);
        }
    }
}
