using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    [CreateAssetMenu(menuName = "Movement/Actions/Patrol")]
    public class MovementPatrolAction : Action
    {
        public override void Act(MovementStateMachine stateMachine, Movementable movementable)
        {
            if (!stateMachine.TryGetComponent<IPatrolRoute>(out var patrolRoute))
            {
                Debug.LogError($"{stateMachine.gameObject.name}: Doesn't have {nameof(IPatrolRoute)}");
                return;
            }

            movementable.SetDirection(patrolRoute.GetDirection());
        }
    }
}
