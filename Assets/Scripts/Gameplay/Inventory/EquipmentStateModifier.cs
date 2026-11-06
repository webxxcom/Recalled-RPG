using Recalled.Gameplay.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.Gameplay
{
    /// <summary>
    /// This class is the stats source for the <see cref="StatsAggregator"/> - it subsribes in OnEnable.
    /// The main purpose of this class is to provide the final stat modifiers for the provided stat definition
    /// It uses <see cref="InventoryHolder"/> to find the equipped items and retrieve the <see cref="EquippableModule"/>
    ///     to get the data for the stat.
    /// The final values are calculated in the aggregator and this class only says that they exist
    /// </summary>
    [RequireComponent(typeof(StatsAggregator))]
    [RequireComponent(typeof(InventoryHolder))]
    internal class EquipmentStateModifier : MonoBehaviour, IStateModifierSource
    {
        StatsAggregator _aggregator;
        InventoryHolder _inventoryHolder;

        public event Action Changed;

        private void Awake()
        {
            _aggregator = GetComponent<StatsAggregator>();
            _inventoryHolder = GetComponent<InventoryHolder>();
        }

        private void OnEnable()
        {
            _aggregator.Register(this);
            _inventoryHolder.Reference.Changed += OnInventoryUpdate;
        }
        private void OnDisable()
        {
            _aggregator.Unregister(this);
            _inventoryHolder.Reference.Changed -= OnInventoryUpdate;
        }

        void OnInventoryUpdate() => Changed?.Invoke();

        public StatModifier[] Retrieve(StatDefinition statDefinition)
        {
            List<StatModifier> res = new();
            foreach (var item in _inventoryHolder.Reference.EquippedItems)
            {
                if (!item.Definition.TryGetModule<EquippableModule>(out var module))
                    continue;

                res.AddRange(module.Stats.Where(s => s.Stat == statDefinition));
            }
            return res.ToArray();
        }
    }
}
