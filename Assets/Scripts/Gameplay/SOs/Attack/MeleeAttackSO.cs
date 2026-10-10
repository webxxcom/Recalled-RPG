using UnityEngine;

[CreateAssetMenu(menuName = "ApplyAttack/Melee Value")]
public class MeleeAttackSO : AttackSO
{
    [SerializeField] int _dealtDamage = 10;
    [SerializeField] float _knockbackPower = 1.6f;
    [SerializeField] float _impactTime = 0.3f;
    [SerializeField] float _recoveryTime = 0.8f;
    [SerializeField] AttackCurvesSO _curves;

    public float ImpactTime => _impactTime;
    public float RecoveryTime => _recoveryTime;

    public void ApplyAttack(GameObject source, Collider2D hurtbox)
    {
        if (hurtbox.TryGetComponent(out HealthResource target))
        {
            DamageInfo di = new(_knockbackPower, _dealtDamage, source, hurtbox);

            target.ApplyDamage(di);
        }
    }

    public void HitboxOverTime(CapsuleCollider2D hitbox, float normalizedTime)
    {
        if (!_curves)
            return;

        hitbox.size = new(
                _curves.ColliderSizeX.length > 1
                ? _curves.ColliderSizeX.Evaluate(normalizedTime)
                : hitbox.size.x,
                _curves.ColliderSizeY.length > 1
                ? _curves.ColliderSizeY.Evaluate(normalizedTime)
                : hitbox.size.y
                );
        hitbox.offset = new(
            _curves.ColliderOffsetX.length > 1
            ? _curves.ColliderOffsetX.Evaluate(normalizedTime)
            : hitbox.offset.x,
              _curves.ColliderOffsetY.length > 1
            ? _curves.ColliderOffsetY.Evaluate(normalizedTime)
            : hitbox.offset.y
            );
    }
}
