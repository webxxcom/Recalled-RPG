using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteraction : MonoBehaviour
{
    readonly HashSet<IInteractable> _interactables = new(16);

    void OnInteract(InputValue _) => _interactables.FirstOrDefault()?.Interact();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
            _interactables.Add(interactable);

        if (collision.TryGetComponent<BehaviorToggler>(out var popup))
            popup.Show();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
            _interactables.Remove(interactable);

        if (collision.TryGetComponent<BehaviorToggler>(out var popup))
            popup.Hide();
    }
}
