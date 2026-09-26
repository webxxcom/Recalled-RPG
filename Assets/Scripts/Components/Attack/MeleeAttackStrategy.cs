using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class MeleeAttackStrategy : AttackStrategy
{
    [SerializeField] MeleeAttackSO _meleeAttackData;
    public override AttackSO AttackData => _meleeAttackData;
    public override int AnimatorHash => AnimatorParameters.MeleeHash;

    protected override bool WithinAttackRange(AttackContext attackContext)
    {
        return _hitbox.IsTouching(attackContext.Target.GetComponent<Collider2D>());
    }

    CapsuleCollider2D _hitbox;
    EntityController _entityController;

    private void Awake()
    {
        _hitbox = GetComponent<CapsuleCollider2D>();
        _entityController = GetComponentInParent<EntityController>();
    }

    readonly List<Collider2D> _processedTargets = new();
    readonly List<Collider2D> _hits = new(10);

    public override void StartExecuting(AttackContext attackContext)
    {
        _elapsedSinceAttack = 0;
    }

    public override void ProcessState(float normalizedTime, AttackContext attackContext)
    {
        if (normalizedTime < _meleeAttackData.ImpactTime || normalizedTime > _meleeAttackData.RecoveryTime || !enabled)
            return;

        _meleeAttackData.HitboxOverTime(_hitbox, normalizedTime);
        _hits.Clear();
        _hitbox.Overlap(_hits);
        foreach (Collider2D hit in _hits)
        {
            if (_processedTargets.Contains(hit) || hit.CompareTag(_hitbox.tag))
                continue;

            _processedTargets.Add(hit);

            _meleeAttackData.ApplyAttack(_entityController, hit);
        }
    }

    public override void FinishExecuting(AttackContext attackContext)
    {
        _processedTargets.Clear();
    }
}
