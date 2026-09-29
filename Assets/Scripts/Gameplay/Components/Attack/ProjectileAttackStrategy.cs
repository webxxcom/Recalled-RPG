using System.Collections.Generic;
using UnityEngine;

class ProjectileAttackStrategy : AttackStrategy
{
    [SerializeField] ProjectileAttackDataSO _projectileAttackData;
    public override AttackSO AttackData => _projectileAttackData;
    public override int AnimatorHash => AnimatorParameters.ShootHash;

    AttackContext _attackContext;
    protected override bool WithinAttackRange(AttackContext attackContext)
    {
        _attackContext = attackContext;

        return (attackContext.Target.GetComponent<Collider2D>().bounds.center - transform.position).sqrMagnitude
            <= _projectileAttackData.Range * _projectileAttackData.Range;
    }

    [SerializeField] AnimationController _animationController;

    EntityController _entityController;
    bool _completed;

    private void Awake()
    {
        _entityController = GetComponentInParent<EntityController>();
    }

    public override void ProcessState(float normalizedTime, AttackContext attackContext)
    {
        if (normalizedTime >= _projectileAttackData.NormalizedSpawnPoint && !_completed)
        {
            GameObject projectile = Instantiate(
                _projectileAttackData.ProjectilePrefab, _entityController.transform.position, Quaternion.identity);

            // TODO trashy init
            projectile.GetComponent<ProjectileScript>().Initialize(
                _entityController,
                attackContext.Target.transform.position,
                _animationController.FlippedX);

            _completed = true;
        }
    }

    public override void StartExecuting(AttackContext attackContext)
    {
        _completed = false;
        _elapsedSinceAttack = 0;
    }

    public override void FinishExecuting(AttackContext attackContext)
    {
        _attackContext = null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_animationController == null)
            _animationController = transform.parent.GetComponentInChildren<AnimationController>();
    }
#endif

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _projectileAttackData.Range);

        if (_attackContext != null)
            Gizmos.DrawLine(_attackContext.Target.GetComponent<Collider2D>().bounds.center, transform.position);
    }
}
