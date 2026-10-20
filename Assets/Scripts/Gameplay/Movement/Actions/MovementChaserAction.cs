using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    [CreateAssetMenu(menuName = "Movement/Actions/Chase")]
    public class MovementChaserAction : Action
    {
        public override void Act(MovementStateMachine stateMachine, Movementable movementable)
        {
            if (!stateMachine.TryGetComponent<IChaser>(out var targetProvider))
            {
                Debug.LogError($"{stateMachine.gameObject.name}: Doesn't have {nameof(IChaser)}");
                return;
            }

            movementable.SetDirection(targetProvider.Direction);
        }
    }
}
