using UnityEngine;

public class DamageInvincibility : HealthReactor
{
    [SerializeField] float _duration;

    protected override void OnHpChanged(DamageInfo di)
        => _health.GrantInvincibility(_duration);
}
