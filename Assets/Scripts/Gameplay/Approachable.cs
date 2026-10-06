using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.Gameplay
{
    public class Approachable : MonoBehaviour
    {
        IApproachReactor[] _reactors;

        IEnumerable<IApproachReactor> ActiveReactors
            => _reactors.Where(r => r.enabled);

        private void Awake()
        {
            _reactors = GetComponentsInChildren<IApproachReactor>(true);
        }

        public event Action<Approachable> Approached;
        public event Action<Approachable> Retreated;

        public void Approaching()
        {
            foreach (var reactor in ActiveReactors)
                reactor.ReactToApproach(this);

            Approached?.Invoke(this);
        }

        public void Retreating()
        {
            foreach (var reactor in ActiveReactors)
                reactor.ReactToRetreat(this);

            Retreated?.Invoke(this);
        }
    }
}
