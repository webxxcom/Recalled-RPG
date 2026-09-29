using UnityEngine;

public class PlayerAttackStateMachine : StateMachineBehaviour
{
    EntityAttack _entityAttack;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_entityAttack)
            _entityAttack = animator.transform.parent.GetComponentInChildren<EntityAttack>();

        _entityAttack.StartAttack();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _entityAttack.ProcessAttack(stateInfo.normalizedTime);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _entityAttack.FinishAttack();
    }
}
