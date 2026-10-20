using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    [CreateAssetMenu(menuName = "Movement/Decision/Lost Interest")]
    public class LostInterestDecision : Decision
    {
        public override bool Decide(MovementStateMachine stateMachine, Movementable movementable)
        {
            if (!stateMachine.TryGetComponent<IChaser>(out var targetProvider))
            {
                Debug.LogError($"{stateMachine.gameObject.name}: Doesn't have {nameof(IChaser)}");
                return false;
            }

            return !targetProvider.HasTarget;
        }
    }
}
