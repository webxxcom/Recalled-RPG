using Recalled.Gameplay;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class InteractableAnimation : MonoBehaviour, IInteractionReactor
{
    [SerializeField] Animator _animator;

    public void ReactToInteraction(Interactable _)
    {
        _animator.SetTrigger(AnimatorParameters.InteractHash);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
    }
#endif
}
