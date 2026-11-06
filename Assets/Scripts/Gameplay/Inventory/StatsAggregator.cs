using Recalled.Gameplay.Inventory;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Recalled.Gameplay
{
    internal sealed class StatsAggregator : MonoBehaviour
    {
        [SerializeField] Dictionary<StatDefinition, float> _baseStats;

        readonly Dictionary<StatDefinition, float> _stats = new();
        readonly HashSet<IStateModifierSource> _modifierSources = new();

        public IReadOnlyDictionary<StatDefinition, float> Stats
        {
            get
            {
                // Update stats only when read
                if (_isDirty)
                {
                    _isDirty = false;
                    UpdateStats();
                }
                return _stats;
            }
        }

        bool _isDirty;
        void OnStatChanged() => _isDirty = true;
        private void OnEnable() => OnStatChanged();

        void UpdateStats()
        {
            _stats.Clear();
            foreach (var stat in _baseStats)
                _stats.Add(stat.Key, GetAggregatedFor(stat.Key));
        }

        float GetAggregatedFor(StatDefinition statDefinition)
        {
            if (!_baseStats.TryGetValue(statDefinition, out var baseValue))
                throw new ArgumentException("Base value for this stat is absent");

            float res = baseValue;
            foreach (var source in _modifierSources)
            {
                float interRes = 0;
                foreach (var statModifier in source.Retrieve(statDefinition))
                    interRes += statModifier.Apply(baseValue);

                res += interRes;
            }
            return res;
        }

        public void Register(IStateModifierSource source)
        {
            _modifierSources.Add(source);
            source.Changed += OnStatChanged;
        }
        public void Unregister(IStateModifierSource source)
        {
            _modifierSources.Remove(source);
            source.Changed -= OnStatChanged;
        }
    }
}
