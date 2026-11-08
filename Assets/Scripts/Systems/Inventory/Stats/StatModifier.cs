using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.Systems.Inventory.Stats
{
    [Serializable]
    public struct StatModifier
    {
        [SerializeField] StatDefinition _stat;
        [SerializeField, Min(0)] float _amount;
        [SerializeField] Operations _operation;

        public readonly StatDefinition Stat => _stat;
        public readonly float Amount => _amount;
        internal readonly Operations Operation => _operation;

        public static string Displayed(StatModifier sm)
        {
            string sign;
            if (sm.Operation == Operations.Add)
            {
                if (sm.Amount > 0) sign = "+";
                else if (sm.Amount < 0) sign = "-";
                else sign = string.Empty;
            }
            else sign = "*";

            return $"{sm.Stat.name}: {sign}{MathF.Abs(sm.Amount)}";
        }
        

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
