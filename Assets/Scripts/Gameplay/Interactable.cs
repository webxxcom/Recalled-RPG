using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    public sealed class Interactable : MonoBehaviour
    {
        IReactor[] _reactors;

        IEnumerable<IReactor> ActiveReactors
            => _reactors.Where(r => r.enabled);

        public Action<InteractionProcessor> RequestInteraction;

        void Awake()
        {
            _reactors = GetComponentsInChildren<IReactor>();
        }

        public void Interact(InteractionProcessor interactor)
        {
            RequestInteraction?.Invoke(interactor);
        }
    }
}
