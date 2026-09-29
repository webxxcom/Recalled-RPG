using UnityEngine;

public class DamageInvincibility : HealthReactor
{
    [SerializeField] float _duration;

    protected override void OnHpChangeApplied(DamageInfo di)
        => _health.GrantInvincibility(_duration);
}
