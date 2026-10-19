using Recalled.Gameplay.Movement;
using System.Timers;
using UnityEngine;

namespace Recalled.Gameplay
{
    public class ZonePatroller : MonoBehaviour, IPatrolRoute
    {
        [SerializeField] Collider2D _zoneCollider;
        [SerializeField] float _interval1 = 1.3f;
        [SerializeField] float _interval2 = 2.4f;

        static readonly Vector2[] Directions =
        {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down
        };

        float _elapsedFromLastSet;
        float _totalWait;
        Vector2 _currentDirection;
        public Vector2 GetDirection() => _currentDirection;

        void SetDirection(Vector2 vec2)
        {
            _currentDirection = vec2;
            _elapsedFromLastSet = 0;
            if (_interval1 < _interval2) _totalWait = Random.Range(_interval1, _interval2);
            else _totalWait = Random.Range(_interval2, _interval1);
        }

        void FindNextDirection(bool findNewDirection)
        {
            var prevDirection = _currentDirection;
            do
                _currentDirection = Directions[Random.Range(0, 3)];
            while (findNewDirection && prevDirection == _currentDirection);

            SetDirection(_currentDirection);
        }

        private void Update()
        {
            _elapsedFromLastSet += Time.deltaTime;

            if (_elapsedFromLastSet >= _totalWait)
                FindNextDirection(true);
        }
    }
}
