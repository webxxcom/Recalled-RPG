using Recalled.Gameplay;
using System;
using UnityEngine;

namespace Recalled.UI
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        IInteractionReactor[] _reactors;

        public bool CanBeInteracted => true;

        public event Action Interacted;

        private void Awake()
        {
            _reactors = GetComponentsInChildren<IInteractionReactor>();
        }

        public void Interact()
        {
            foreach (var reactor in _reactors)
                reactor.ReactToInteraction(this);

            Interacted?.Invoke();
        }
    }
}
