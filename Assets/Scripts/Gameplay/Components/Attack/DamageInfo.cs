using System;
using UnityEngine;

namespace Recalled.Gameplay.Combat
{
    public readonly struct DamageInfo
    {
        public readonly float KnockbackPower;
        public readonly int Amount;
        public readonly GameObject Source;
        public readonly Health Target;
        public readonly Vector2 Direction;

        public DamageInfo(float knockbackPower, int amount, GameObject source, Health health)
        {
            if (source == null || health == null)
                throw new ArgumentNullException();

            Amount = amount;
            KnockbackPower = knockbackPower;
            Source = source;
            Target = health;
            Direction = (Target.Center - source.GetComponent<Health>().Center).normalized;
        }
    }
}