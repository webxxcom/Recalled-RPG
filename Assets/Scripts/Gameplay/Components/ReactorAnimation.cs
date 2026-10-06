using Recalled.Gameplay;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ReactorAnimation : MonoBehaviour, IReactor
{
    [SerializeField] Animator _animator;

    public void React()
    {
        _animator.SetTrigger(AnimatorParameters.InteractHash); // TODO need to change the hash
    }
}
