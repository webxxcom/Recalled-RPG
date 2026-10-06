using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Lootable))]
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] ItemDefinition _requiredKey;
        [SerializeField] InventorySO _inventory;

        Lootable _lootable;

        public bool CanBeInteracted
            => enabled && (_requiredKey == null || _inventory.GeneralItems.Has(_requiredKey));

        public event Action Interacted;

        void Awake()
        {
            _lootable = GetComponent<Lootable>();
        }

        public void Interact()
        {
            if (!CanBeInteracted)
                return;

            var looted = _lootable.LootItem();
            if (_inventory.GeneralItems.Add(new(looted.Definition, looted.Count)))
                _inventory.GeneralItems.Remove(_requiredKey);

            Interacted?.Invoke();
            enabled = false;
        }

        public void ReactToApproach(IApproachable approachable)
        {
            throw new System.NotImplementedException();
        }

        public void ReactToRetreat(IApproachable approachable)
        {
            throw new System.NotImplementedException();
        }
    }
}
