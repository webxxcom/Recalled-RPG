using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay.Assets.Scripts.Gameplay.Objects
{
    public class Lever : MonoBehaviour
    {
        [Header("Edge events")]
        [SerializeField] UnityEvent _onOff;
        [SerializeField] Animator _animator;

        public enum States { Opened, Closed }
        public States State { get; private set; } = States.Closed;
        void Start() => ApplyLevels();

        void ToggleState()
        {
            if (State == States.Opened) SetState(States.Closed);
            else SetState(States.Opened);
        }

        public void SetState(States state)
        {
            State = state;
            ApplyLevels();
        }

        void ApplyLevels()
        {
            bool open = State == States.Opened;

            if (_animator != null)
                _animator.SetBool(AnimatorParameters.IsOpenHash, open);
        }
    }
}
