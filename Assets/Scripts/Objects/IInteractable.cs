using System;

public interface IInteractable
{
    public event Action OnInteract;

    void Interact();
}
