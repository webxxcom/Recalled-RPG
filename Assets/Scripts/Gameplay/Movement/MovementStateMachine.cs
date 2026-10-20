using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Movementable))]
    [RequireComponent(typeof(EntityIdentity))]
    public class MovementStateMachine : MonoBehaviour
    {
        [SerializeField] State _remainState;
        [SerializeField] State _defaultState;

        Movementable _movementable;
        State _firstState;
        State _currentState;

        private void Awake()
        {
            _movementable = GetComponent<Movementable>();

            _firstState = GetComponent<EntityIdentity>().Definition.MovementState;
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
