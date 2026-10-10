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
        [SerializeField] Animator _animator;

        [Header("Edges: fire once, on the change, never on load")]
        [SerializeField] UnityEvent _opened;
        [SerializeField] UnityEvent _rejected;

        [Header("Levels: current state, sent at start and on every change")]
        // true while the chest can still be opened, e.g. bind ApproachPopup.enabled (dynamic bool).
        [SerializeField] UnityEvent<bool> _openableChanged;

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

        public void Interact(InteractionProcessor interactor)
        {
            if (State == States.Opened)
                return;

            var inventory = interactor.GetComponentInParent<InventoryHolder>().Inventory;
            var loot = _lootTable.GetItem();

            bool hasKey = _requiredKey == null || inventory.Take(_requiredKey, 1);
            if (!hasKey)
            {
                _rejected.Invoke();
                return;
            }
            
            inventory.Add(loot);

            State = States.Opened;
            ApplyLevels();
            _opened.Invoke();
        }

        public void SetState(States state)
        {
            State = state;
            ApplyLevels();
        }

        void ApplyLevels()
        {
            bool open = State == States.Opened;

            if (_animator != null)
                _animator.SetBool(AnimatorParameters.IsOpenHash, open);

            _openableChanged.Invoke(!open);
        }
    }
}
