using System;
using UnityEngine;

public readonly struct DamageInfo
{
    public readonly float KnockbackPower;
    public readonly int Amount;
    public readonly GameObject Source;
    public readonly Collider2D Hurtbox;
    public readonly Vector2 Direction;

    public DamageInfo(float knockbackPower, int amount, GameObject source, Collider2D hurtbox)
    {
        if (source == null || hurtbox == null)
            throw new ArgumentNullException();

        Amount = amount;
        KnockbackPower = knockbackPower;
        Source = source;
        Hurtbox = hurtbox;
        Direction =
            (Hurtbox.bounds.center - source.GetComponentInChildren<HealthResource>().Hurtbox.bounds.center).normalized;
    }
}