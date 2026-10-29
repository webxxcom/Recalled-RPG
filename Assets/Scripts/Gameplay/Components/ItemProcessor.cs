using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(InventoryHolder))]
    [RequireComponent(typeof(QuickSlotsHolder))]
    internal class ItemProcessor : MonoBehaviour, IItemProcessor
    {
        [SerializeField] ItemProcessorAnchor _processorAnchor;

        InventoryHolder _inventory;
        QuickSlotsHolder _quickSlots;

        private void Awake()
        {
            _inventory = GetComponent<InventoryHolder>();
            _quickSlots = GetComponent<QuickSlotsHolder>();
        }
        private void OnEnable() => _processorAnchor.Provide(this);
        private void OnDisable() => _processorAnchor.Withdraw(this);

        public bool Consume(ItemDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException();

            bool consumed = false;
            if (_inventory.Reference.Has(definition, 1) && definition.TryGetModule<Consumable>(out var module))
            {
                if (module.Consume(gameObject))
                {
                    _inventory.Reference.Take(definition, 1);
                    consumed = true;
                }
            }
            return consumed;
        }

        public bool Consume(ItemInstance item)
        {
            if (item == null) throw new ArgumentNullException();

            bool consumed = false;
            if (item.Count > 0 && item.Definition.TryGetModule<Consumable>(out var module))
            {
                if (module.Consume(gameObject))
                {
                    _inventory.Reference.Take(item, 1);
                    consumed = true;
                }
            }
            return consumed;
        }

        public bool TryDiscard(ItemInstance item)
            => _inventory.Reference.Remove(item);
    }
}
