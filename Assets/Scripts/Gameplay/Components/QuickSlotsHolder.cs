using NUnit.Framework;
using Recalled.Systems.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(InventoryHolder))]
    sealed class QuickSlotsHolder : MonoBehaviour
    {
        public class QuickSlots
        {
            public class Slot
            {
                public readonly ItemDefinition Definition;
                public int Count;

                public Slot(ItemDefinition definition, int count)
                {
                    Definition = definition;
                    Count = count;
                }
            }

            readonly Bag _inventory;
            readonly Slot[] _slots = new Slot[4];

            public QuickSlots(Bag bag)
            {
                _inventory = bag;

                _inventory.SlotsChanged += UpdateAll;
            }

            void UpdateAll()
            {
                for (int i = 0; i < _slots.Length; i++)
                {
                    if (_slots[i] == null)
                        continue;

                    var slot = _slots[i];

                    _slots[i].Count =
                        _inventory.Slots
                            .Where(s => !s.IsEmpty && s.Item.Definition == slot.Definition)
                            .Sum(s => s.Item.Count);
                }
            }

            public void Put(int i, ItemDefinition def)
            {
                if (def == null) throw new ArgumentNullException();

                _slots[i] = new(def, _inventory.Slots.Where(s => !s.IsEmpty && s.Item.Definition == def).Sum(s => s.Item.Count));
            }
            public void Remove(int i)
            {
                _slots[i] = default;
            }

            public void Use(int i, GameObject consumer)
            {
                if (_slots[i] == null) return;

                if (!_slots[i].Definition.TryGetModule<Consumable>(out var consumeModule))
                    return;

                if (_inventory.Take(_slots[i].Definition, 1))
                {
                    _slots[i].Count--;
                    consumeModule.Consume(consumer);
                }
            }
        }

        [SerializeField] InputActionReference _useQuickSlotAction;
        [SerializeField] List<ItemDefinition> _loadout;

        QuickSlots _quickSlots;

        private void Awake() => _quickSlots = new(GetComponent<InventoryHolder>().Inventory);
        private void Start()
        {
            int i = 0;
            foreach (var item in _loadout)
                _quickSlots.Put(i++, item);
        }
        private void OnEnable() => _useQuickSlotAction.action.performed += OnQuickSlotAction;
        private void OnDisable() => _useQuickSlotAction.action.performed -= OnQuickSlotAction;

        void OnQuickSlotAction(InputAction.CallbackContext context)
        {
            var vec2 = context.ReadValue<Vector2>();

            int i = -1;
            if (vec2 == Vector2.left) i = 0;
            else if (vec2 == Vector2.up) i = 1;
            else if (vec2 == Vector2.right) i = 2;
            else if (vec2 == Vector2.down) i = 3;

            if (i == -1)
                return;

            _quickSlots.Use(i, gameObject);
        }
    }
}
