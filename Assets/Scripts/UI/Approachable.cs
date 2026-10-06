using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    public class Approachable : MonoBehaviour, IApproachable
    {
        IApproachReactor[] _reactors;

        private void Awake()
        {
            _reactors = GetComponentsInChildren<IApproachReactor>(true);
        }

        public event Action<IApproachable> Approached;
        public event Action<IApproachable> Retreated;

        public void Approaching()
        {
            foreach (var reactor in _reactors)
                reactor.ReactToApproach(this);

            Approached?.Invoke(this);
        }

        public void Retreating()
        {
            foreach (var reactor in _reactors)
                reactor.ReactToRetreat(this);

            Retreated?.Invoke(this);
        }
    }
}
