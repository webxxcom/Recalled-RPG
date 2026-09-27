using System;
using UnityEngine;

public class NpcController : EntityController, IInteractable
{
    [SerializeField] DialogueSource _dialogueSource;

    [Header("Sets")]
    [SerializeField] DialogueVariable _currentDialogue;

    public event Action OnInteract;

    public void Interact()
    {
        _currentDialogue.Value = _dialogueSource;

        OnInteract?.Invoke();
    }
}
