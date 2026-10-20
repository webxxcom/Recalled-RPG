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
        [SerializeField] States _state = States.Closed;

        [Header("Edge events")]
        [SerializeField] UnityEvent _openClose;
        [SerializeField] Animator _animator;

        Collider2D _collider2D;

        public enum States { Open, Closed }

        public void ToggleState()
        {
            if (_state == States.Open) SetState(States.Closed);
            else SetState(States.Open);
        }
        private void Awake() => _collider2D = GetComponent<Collider2D>();
        private void Start() => ApplyLevels();

        public void SetState(States state)
        {
            foreach (var condition in _openConditions)
            {
                if (!condition.Check())
                    return;
            }

            _state = state;
            ApplyLevels();
        }

        void ApplyLevels()
        {
            bool open = _state == States.Open;

            _collider2D.enabled = !open;
            if (_animator != null)
                _animator.SetBool(AnimatorParameters.IsOpenHash, open);
        }
    }
}
