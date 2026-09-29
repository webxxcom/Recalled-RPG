using UnityEngine;

public class DamageKnockback : HealthReactor
{
    [SerializeField] ExternalVelocity _externalVelocity;

    protected override void OnHpChangeApplied(DamageInfo di)
        => _externalVelocity.Add(di.Direction * di.KnockbackPower);
}
