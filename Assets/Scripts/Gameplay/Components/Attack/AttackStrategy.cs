using UnityEngine;

[RequireComponent(typeof(EntityAttack))]
public abstract class AttackStrategy : MonoBehaviour
{
    public abstract int AnimatorHash { get; }
    public abstract AttackSO AttackData { get; }
    protected float _elapsedSinceAttack;

    public bool CanAttack(AttackContext attackContext)
        => _elapsedSinceAttack >= AttackData.ReloadTime && WithinAttackRange(attackContext);
    protected abstract bool WithinAttackRange(AttackContext attackContext);

    public abstract void StartExecuting(AttackContext attackContext);
    public abstract void ProcessState(float normalizedTime, AttackContext attackContext);
    public abstract void FinishExecuting(AttackContext attackContext);

    private void Update()
    {
        _elapsedSinceAttack += Time.deltaTime;
    }
}
