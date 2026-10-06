using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    public class Interactable : MonoBehaviour
    {
        IReactor[] _reactors;

        IEnumerable<IReactor> ActiveReactors
            => _reactors.Where(r => r.enabled);

        public virtual bool CanBeInteracted => true;
        public event Action Interacted;

        protected virtual void Awake()
        {
            _reactors = GetComponentsInChildren<IReactor>();
        }

        public virtual void Interact()
        {
            foreach (var reactor in ActiveReactors)
                reactor.React();

            Interacted?.Invoke();
        }
    }
}
