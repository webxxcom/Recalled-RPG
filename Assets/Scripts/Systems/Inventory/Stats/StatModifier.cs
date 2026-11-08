using System;
using UnityEngine;

namespace Recalled.Systems.Inventory.Stats
{
    [Serializable]
    public struct StatModifier
    {
        [SerializeField] StatDefinition _stat;
        [SerializeField] float _amount;
        [SerializeField] Operations _operation;

        public readonly StatDefinition Stat => _stat;

        /// <summary>
        /// Applies this stat data to the base value depending on its aggregation operation <see cref="Operations"/>
        /// </summary>
        /// <param name="baseVal">The base value multiply for if necessary</param>
        /// <returns>the value to add to the base value</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public readonly float Apply(float baseVal)
        {
            return _operation switch
            {
                Operations.Add => _amount,
                Operations.Multiply => baseVal * _amount,
                _ => throw new InvalidOperationException($"Unknow Arithmetic operation for ${nameof(StatModifier)}")
            };
        }
    }
}
