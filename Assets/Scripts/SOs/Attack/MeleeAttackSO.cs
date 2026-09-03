using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ApplyAttack/Melee Data")]
public class MeleeAttackSO : AttackSO
{
    [field: SerializeField] public int DealtDamage { get; private set; } = 10;
    [field: SerializeField] public float KnockbackPower { get; private set; } = 1.6f;
    [field: SerializeField] public float ImpactTime { get; private set; } = 0.3f;
    [field: SerializeField] public float RecoveryTime { get; private set; } = 0.8f;
    [field: SerializeField] public AttackCurvesSO Curves { get; private set; }
    [SerializeField] PlayerCombatData _combatData;

    public void ApplyAttack(EntityController source, Collider2D hurtbox)
    {
        if (hurtbox.TryGetComponent(out HealthResource target))
        {
            DamageInfo di = _combatData != null 
                ? new(_combatData.DealtDamage, _combatData.KnockbackPower, source, hurtbox)
                : new(DealtDamage, KnockbackPower, source, hurtbox);

            target.ApplyDamage(di);
        }
    }

    public void HitboxOverTime(CapsuleCollider2D hitbox, float normalizedTime)
    {
        if (!Curves)
            return;

        hitbox.size = new(
                Curves.ColliderSizeX.length > 1
                ? Curves.ColliderSizeX.Evaluate(normalizedTime)
                : hitbox.size.x,
                Curves.ColliderSizeY.length > 1
                ? Curves.ColliderSizeY.Evaluate(normalizedTime)
                : hitbox.size.y
                );
        hitbox.offset = new(
            Curves.ColliderOffsetX.length > 1
            ? Curves.ColliderOffsetX.Evaluate(normalizedTime)
            : hitbox.offset.x,
              Curves.ColliderOffsetY.length > 1
            ? Curves.ColliderOffsetY.Evaluate(normalizedTime)
            : hitbox.offset.y
            );
    }
}
