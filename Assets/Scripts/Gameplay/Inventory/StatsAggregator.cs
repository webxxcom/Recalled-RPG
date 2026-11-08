using Recalled.Systems.Inventory.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class StatsAggregator : MonoBehaviour
    {
        [SerializeField] Dictionary<StatDefinition, float> _baseStats;
        [SerializeField] StatsAggregatorAnchor _statsAggregatoAnchor;

        readonly Dictionary<StatDefinition, float> _stats = new();
        readonly HashSet<IStateModifierSource> _modifierSources = new();

        public event Action Changed;
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
        void OnStatChanged()
        {
            _isDirty = true;
            Changed?.Invoke();
        }
        private void OnEnable() => OnStatChanged();

        private void Awake()
        {
            _statsAggregatoAnchor.Provide(this);
        }
        private void OnDestroy()
        {
            _statsAggregatoAnchor.Withdraw(this);
        }

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

        internal void Register(IStateModifierSource source)
        {
            _modifierSources.Add(source);
            source.Changed += OnStatChanged;
        }
        internal void Unregister(IStateModifierSource source)
        {
            _modifierSources.Remove(source);
            source.Changed -= OnStatChanged;
        }
    }
}
