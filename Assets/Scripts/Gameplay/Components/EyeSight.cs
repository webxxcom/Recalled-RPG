using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class EyeSight : MonoBehaviour
    {
        [SerializeField] bool _drawGizmos;
        [SerializeField] LayerMask _layerMask;

        CircleCollider2D _collider2D;
        readonly HashSet<GameObject> _overlaps = new();

        public event Action<GameObject> SawSomeone;

        private void Awake() => _collider2D = GetComponent<CircleCollider2D>();

        readonly RaycastHit2D[] hits = new RaycastHit2D[1];
        private void OnTriggerStay2D(Collider2D collision)
        {
            var hitsN = Physics2D.RaycastNonAlloc(
                _collider2D.bounds.center,
                Vector2.Normalize(collision.transform.position - transform.position),
                hits,
                _collider2D.radius,
                _layerMask.value);

            if (hitsN > 0)
                return;

            if (_overlaps.Add(collision.gameObject))
                SawSomeone?.Invoke(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _overlaps.Remove(collision.gameObject);
        }

        public GameObject FindClosest()
        {
            GameObject closest = default;
            float minDist = float.MaxValue;

            foreach (var overlap in _overlaps)
            {
                var dist = Vector2.Distance(transform.position, overlap.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = overlap;
                }
            }
            return closest;
        }

        public bool IsWithinSight(GameObject game) => _overlaps.Contains(game);

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;

            foreach (var overlap in _overlaps)
                Gizmos.DrawLine(transform.position, overlap.transform.position);
        }
    }
}
