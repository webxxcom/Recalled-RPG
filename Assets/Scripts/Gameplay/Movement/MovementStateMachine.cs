using Recalled.Gameplay.Movement;
using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Movementable))]
    public class MovementStateMachine : MonoBehaviour
    {
        [SerializeField] State _firstState;
        [SerializeField] State _remainState;
        [SerializeField] State _defaultState;

        Movementable _movementable;
        State _currentState;

        private void Awake()
        {
            _movementable = GetComponent<Movementable>();

            _currentState = _firstState;
        }

        public void TransitionToState(State state)
        {
            if (state == _remainState) return;
            if (state == _defaultState)
                state = _firstState;

            _currentState = state;
        }

        private void Update()
        {
            _currentState.UpdateState(this, _movementable);
        }
    }
}
