using Recalled.Gameplay.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.Gameplay
{
    internal sealed class StatsAggregator : MonoBehaviour
    {
        [SerializeField] Dictionary<StatDefinition, float> _baseStats;

        readonly HashSet<IStateModifierSource> _modifierSources = new();

        public float GetAggregatedFor(StatDefinition statDefinition)
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

        public void Register(IStateModifierSource source) => _modifierSources.Add(source);
        public void Unregister(IStateModifierSource source) => _modifierSources.Remove(source);
    }
}
