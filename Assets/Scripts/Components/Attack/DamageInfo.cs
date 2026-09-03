using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
    public float KnockbackPower { get; private set; }
    public int Amount { get; set; }
    public EntityController Source { get; private set; }
    public Collider2D Hurtbox { get; private set; }
    public Vector2 Direction { get; private set; }

    public DamageInfo(int quantity, float knockbackPower, EntityController source,
        Collider2D hurtbox)
    {
        Amount = quantity;
        KnockbackPower = knockbackPower;
        Source = source;
        Hurtbox = hurtbox;
        Direction =
            (Hurtbox.bounds.center - source.GetComponentInChildren<HealthResource>().Hurtbox.bounds.center).normalized;
    }
}