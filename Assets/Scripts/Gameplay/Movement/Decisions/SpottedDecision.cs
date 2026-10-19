using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    [CreateAssetMenu(menuName ="Movement/Decision/Spotted")]
    public class SpottedDecision : Decision
    {
        public override bool Decide(MovementStateMachine stateMachine, Movementable movementable)
        {
            if (!stateMachine.TryGetComponent<ITargetProvider>(out var targetProvider))
            {
                Debug.LogError($"{stateMachine.gameObject.name}: Doesn't have {nameof(ITargetProvider)}");
                return false;
            }

            return targetProvider.HasTarget;
        }
    }
}
