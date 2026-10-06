using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Lootable))]
    public sealed class Chest : Interactable
    {
        [SerializeField] ItemDefinition _requiredKey;
        [SerializeField] InventorySO _inventory;

        Lootable _lootable;

        public enum States
        {
            Opened,
            Rejected,
            Closed
        }
        public States State { get; private set; } = States.Closed;

        public override bool CanBeInteracted
            => enabled && (_requiredKey == null || _inventory.GeneralItems.Has(_requiredKey));

        protected override void Awake()
        {
            base.Awake();

            _lootable = GetComponent<Lootable>();
        }

        public override void Interact()
        {
            if (!CanBeInteracted)
                return;

            var looted = _lootable.LootItem();
            if (_inventory.GeneralItems.Add(new(looted.Definition, looted.Count)))
            {
                // Successful interaction only if item can be added
                State = States.Opened;
                _inventory.GeneralItems.Remove(_requiredKey);
                enabled = false;
            }
            else State = States.Rejected;

            // Even if interaction is not successful reactors check the Chest.State
            base.Interact();
        }
    }
}
