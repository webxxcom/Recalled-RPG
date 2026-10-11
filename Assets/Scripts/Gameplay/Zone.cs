using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    public class Zone : MonoBehaviour
    {
        [SerializeField] Collider2D _detectionZone;
        [SerializeField] LayerMask _trackedLayers;

        public UnityEvent<GameObject> Approached;
        public UnityEvent Retreated;

        readonly HashSet<Collider2D> _inZone = new();
        public bool HasAnyone => _inZone.Count != 0;

        IZoneReactor[] _reactors;

        IEnumerable<IZoneReactor> ActiveReactors
            => _reactors.Where(r => r.enabled);

        private void Awake()
        {
            _reactors = GetComponentsInChildren<IZoneReactor>(true);
        }

        private void Start()
        {
            _detectionZone.contactCaptureLayers = _trackedLayers;

            ApplyLevels();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool wasEmpty = _inZone.Count == 0;
            bool entered = _inZone.Add(collision);

            if (entered && wasEmpty)
                Occupied();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _inZone.Remove(collision);

            if (!HasAnyone)
                Free();
        }

        void Occupied()
        {
            foreach (var reactor in ActiveReactors)
                reactor.Present(this);
            Approached.Invoke(gameObject);
        }

        void Free()
        {
            foreach (var reactor in ActiveReactors)
                reactor.Absent(this);
            Retreated.Invoke();
        }

        void ApplyLevels()
        {
            foreach (var reactor in ActiveReactors.Where(r => r.IsLevel))
            {
                if (HasAnyone) reactor.Present(this);
                else reactor.Absent(this);
            }
        }
    }
}
