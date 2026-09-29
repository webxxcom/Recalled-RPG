using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(IInteractable))]
public class InteractableAnimation : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] IInteractable _interactable;

    private void OnEnable()
        => _interactable.OnInteract += OnInteract;
    private void OnDisable()
        => _interactable.OnInteract -= OnInteract;
    void OnInteract()
        => _animator.SetTrigger(AnimatorParameters.InteractHash);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        if (_interactable == null)
            _interactable = GetComponent<IInteractable>();
    }
#endif
}
