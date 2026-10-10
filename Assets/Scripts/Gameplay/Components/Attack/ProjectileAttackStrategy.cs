using UnityEngine;

class ProjectileAttackStrategy : AttackStrategy
{
    [SerializeField] ProjectileAttackDataSO _projectileAttackData;
    [SerializeField] AnimationController _animationController;

    public override AttackSO AttackData => _projectileAttackData;
    public override int AnimatorHash => AnimatorParameters.ShootHash;

    bool _completed;

    protected override bool WithinAttackRange(AttackContext attackContext)
    {
        return (attackContext.Target.GetComponent<Collider2D>().bounds.center - transform.position).sqrMagnitude
            <= _projectileAttackData.Range * _projectileAttackData.Range;
    }

    public override void ProcessState(float normalizedTime, AttackContext attackContext)
    {
        if (normalizedTime >= _projectileAttackData.NormalizedSpawnPoint && !_completed)
        {
            Instantiate(_projectileAttackData.ProjectilePrefab, transform.parent.position, Quaternion.identity)
                .Initialize(
                transform.parent.gameObject,
                attackContext.Target.transform.position,
                _animationController.FlippedX);

            _completed = true;
        }
    }

    public override void StartExecuting(AttackContext _)
    {
        _completed = false;
        _elapsedSinceAttack = 0;
    }

    public override void FinishExecuting(AttackContext _)
    {
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_animationController == null)
            _animationController = transform.parent.GetComponentInChildren<AnimationController>();
    }
#endif
}
