using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    public readonly struct DamageInfo
    {
        public readonly float KnockbackPower;
        public readonly int Amount;
        public readonly GameObject Source;
        public readonly HealthResource Target;
        public readonly Vector2 Direction;

        public DamageInfo(float knockbackPower, int amount, GameObject source, HealthResource health)
        {
            if (source == null || health == null)
                throw new ArgumentNullException();

            Amount = amount;
            KnockbackPower = knockbackPower;
            Source = source;
            Target = health;
            Direction =
                (Target.Hurtbox.bounds.center - source.GetComponentInChildren<HealthResource>().Hurtbox.bounds.center).normalized;
        }
    }
}