using Recalled.Gameplay.Movement;
using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class ZoneWanderer : MonoBehaviour, IDirectionProvider
    {
        [Header("Wandering")]
        [SerializeField, Min(0f)] float _minHold = 1.3f;
        [SerializeField, Min(0f)] float _maxHold = 2.4f;
        [Header("Returning")]
        [SerializeField, Min(0f)] float _arriveDistance = 2f;

        enum State { Wandering, Returning }

        static readonly Vector2[] Directions =
        {
            Vector2.left, Vector2.right, Vector2.up, Vector2.down
        };

        State _state;
        float _holdRemaining;
        bool _shouldIdle;
        Collider2D _zone;
        public Vector2 Direction { get; private set; }

        private void Start()
        {
            _zone = GetComponentInParent<Zone>().Collider2D;

            PickWanderDirection();
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Wandering: TickWandering(); break;
                case State.Returning: TickReturning(); break;
            }
        }

        void PickWanderDirection()
        {
            _shouldIdle = !_shouldIdle;

            if (_shouldIdle)
            {
                Direction = Vector2.zero;
            }
            else
            {
                var prev = Direction;
                do
                    Direction = Directions[Random.Range(0, Directions.Length)];
                while (prev == Direction);
            }

            _holdRemaining = Random.Range(_minHold, _maxHold);
            _state = State.Wandering;
        }

        void TickWandering()
        {
            if (!_zone.OverlapPoint(transform.position))
            {
                EnterReturning();
                return;
            }

            _holdRemaining -= Time.deltaTime;
            if (_holdRemaining <= 0f)
                PickWanderDirection();
        }

        void EnterReturning()
        {
            _state = State.Returning;
            TickReturning();
        }

        void TickReturning()
        {
            var toCenter = (Vector2)_zone.bounds.center - (Vector2)transform.position;

            if (toCenter.sqrMagnitude <= _arriveDistance * _arriveDistance)
            {
                PickWanderDirection();
                return;
            }

            Direction = toCenter.normalized;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_minHold > _maxHold)
                (_minHold, _maxHold) = (_maxHold, _minHold);
        }
        void OnDrawGizmosSelected()
        {
            if (_zone == null) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_zone.bounds.center, _arriveDistance);
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)Direction);
        }
#endif
    }
}
