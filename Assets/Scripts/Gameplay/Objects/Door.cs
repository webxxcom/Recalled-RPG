using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour
    {
        [SerializeReference, SubclassSelector] List<ICondition> _openConditions;

        [Header("Edge events")]
        [SerializeField] UnityEvent _openClose;
        [SerializeField] Animator _animator;

        public enum States { Opened, Closed }
        public States State { get; private set; } = States.Closed;

        void ToggleState()
        {
            if (State == States.Opened) SetState(States.Closed);
            else SetState(States.Opened);
        }

        public void SetState(States state)
        {
            foreach (var condition in _openConditions)
            {
                if (!condition.Check())
                    return;
            }

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
