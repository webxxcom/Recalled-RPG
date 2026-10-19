using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    [CreateAssetMenu(menuName = "Movement/State")]
    public class State : ScriptableObject
    {
        [SerializeField] Action[] _actions;
        [SerializeField] Transition[] _transitions;

        public void UpdateState(MovementStateMachine stateMachine, Movementable movementable)
        {
            DoActions(stateMachine, movementable);
            DoTransitions(stateMachine, movementable);
        }

        void DoActions(MovementStateMachine stateMachine, Movementable movementable)
        {
            foreach (var action in _actions)
            {
                action.Act(stateMachine, movementable);
            }
        }

        void DoTransitions(MovementStateMachine stateMachine, Movementable movementable)
        {
            foreach (var transition in _transitions)
            {
                if (transition.Decision.Decide(stateMachine, movementable))
                {
                    stateMachine.TransitionToState(transition.TrueState);
                }
                else stateMachine.TransitionToState(transition.FalseState);
            }
        }
    }
}
