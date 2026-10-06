using System;

namespace Recalled.Gameplay
{
    public interface IInteractable
    {
        public event Action Interacted;
        public bool CanBeInteracted { get; }

        void Interact();
    }
}
