using Recalled.Systems.Inventory;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Interactable))]
    public sealed class Chest : MonoBehaviour
    {
        [SerializeField] ItemDefinition _requiredKey;
        [SerializeField] LootTable _lootTable;

        [Header("Edge events")]
        [SerializeField] UnityEvent _openClose;
        [SerializeField] UnityEvent _rejected;

        // Animator is the tied to the chest because opening means opening the lid
        [SerializeField] Animator _animator;

        Interactable _interactable;

        public enum States { Opened, Closed }
        public States State { get; private set; } = States.Closed;

        void Awake()
        {
            if (!_lootTable.IsValid)
                throw new ArgumentException("Loot table is not valid");

            _interactable = GetComponent<Interactable>();
        }
        void OnEnable() => _interactable.RequestInteraction += Interact;
        void OnDisable() => _interactable.RequestInteraction -= Interact;
        void Start() => ApplyLevels();

        bool _wasOpened;
        public void Interact(InteractionProcessor interactor)
        {
            if (!_wasOpened)
            {
                var inventory = interactor.GetComponentInParent<InventoryHolder>().Inventory;
                var loot = _lootTable.GetItem();

                bool hasKey = _requiredKey == null || inventory.Take(_requiredKey, 1);
                if (!hasKey)
                {
                    _rejected.Invoke();
                    return;
                }

                inventory.Add(loot);
            }

            _openClose.Invoke();
            ToggleState();
        }

        void ToggleState()
        {
            if (State == States.Opened) SetState(States.Closed);
            else SetState(States.Opened);
        }

        public void SetState(States state)
        {
            State = state;
            ApplyLevels();
        }

        void ApplyLevels()
        {
            bool open = State == States.Opened;

            if (!_wasOpened) 
                _wasOpened = open;
            if (_animator != null)
                _animator.SetBool(AnimatorParameters.IsOpenHash, open);
        }
    }
}
