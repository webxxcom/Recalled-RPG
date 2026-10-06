using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    public class Interactable : MonoBehaviour
    {
        IInteractionReactor[] _reactors;

        IEnumerable<IInteractionReactor> ActiveReactors
            => _reactors.Where(r => r.enabled);

        public virtual bool CanBeInteracted => true;
        public event Action Interacted;

        protected virtual void Awake()
        {
            _reactors = GetComponentsInChildren<IInteractionReactor>();
        }

        public virtual void Interact()
        {
            foreach (var reactor in ActiveReactors)
                reactor.ReactToInteraction(this);

            Interacted?.Invoke();
        }
    }
}
