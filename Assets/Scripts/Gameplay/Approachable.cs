using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    public class Approachable : MonoBehaviour
    {
        [SerializeField] Collider2D _detectionZone;
        [SerializeField] LayerMask _trackedLayers;

        public UnityEvent<GameObject> Approached;
        public UnityEvent Retreated;

        readonly HashSet<Collider2D> _inZone = new();

        IApproachReactor[] _reactors;

        IEnumerable<IApproachReactor> ActiveReactors
            => _reactors.Where(r => r.enabled);

        private void Awake()
        {
            _reactors = GetComponentsInChildren<IApproachReactor>(true);
        }

        private void Start()
        {
            _detectionZone.contactCaptureLayers = _trackedLayers;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _inZone.Add(collision);
            if (_inZone.Count == 1) // First one
                Approaching();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _inZone.Remove(collision);
            if (_inZone.Count == 0) // Last exited
                Retreating();
        }

        void Approaching()
        {
            foreach (var reactor in ActiveReactors)
                reactor.ReactToApproach(this);
            Approached.Invoke(gameObject);
        }

        void Retreating()
        {
            foreach (var reactor in ActiveReactors)
                reactor.ReactToRetreat(this);
            Retreated.Invoke();
        }
    }
}
