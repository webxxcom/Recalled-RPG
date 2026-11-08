using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using Recalled.Systems.Inventory.Stats;
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
    [RequireComponent(typeof(EquipmentHolder))]
    internal class EquipmentStatsModifier : MonoBehaviour, IStateModifierSource
    {
        StatsAggregator _aggregator;
        EquipmentHolder _equipmentHolder;

        public event Action Changed;

        private void Awake()
        {
            _aggregator = GetComponent<StatsAggregator>();
            _equipmentHolder = GetComponent<EquipmentHolder>();
        }

        private void OnEnable()
        {
            _aggregator.Register(this);
            _equipmentHolder.Reference.Changed += OnEquipmentChange;
        }
        private void OnDisable()
        {
            _aggregator.Unregister(this);
            _equipmentHolder.Reference.Changed -= OnEquipmentChange;
        }

        void OnEquipmentChange()
        {
            Changed?.Invoke();
        }

        public StatModifier[] Retrieve(StatDefinition statDefinition)
        {
            return _equipmentHolder.Reference.Equipped
                .SelectMany(ei =>  ei.Equippable.Stats.Where(sm => sm.Stat == statDefinition))
                .ToArray();
        }
    }
}
