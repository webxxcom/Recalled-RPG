using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    [CreateAssetMenu(menuName = "Movement/Actions/Chase")]
    public class MovementChaserAction : Action
    {
        public override void Act(MovementStateMachine stateMachine, Movementable movementable)
        {
            if (!stateMachine.TryGetComponent<ITargetProvider>(out var targetProvider))
            {
                Debug.LogError($"{stateMachine.gameObject.name}: Doesn't have {nameof(ITargetProvider)}");
                return;
            }

            Vector2 dir = targetProvider.Target != null
                ? Vector2.Normalize(targetProvider.Target.transform.position - stateMachine.transform.position)
                : Vector2.zero;

            movementable.SetDirection(dir);
        }
    }
}
