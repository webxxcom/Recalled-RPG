using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [Serializable]
    public sealed class HealingEffect : IEffect
    {
        [SerializeField] int _healingPower;

        public void Apply(GameObject game)
        {
            if (!game.TryGetComponent<Health>(out var health))
                return;

            health.Heal(_healingPower);
        }

        public bool Check(GameObject game)
        {
            if (!game.TryGetComponent<Health>(out var health))
                return false;

            return !health.IsFull;
        }
    }
}
