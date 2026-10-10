using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class AnimateDeath : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] float _timeout = 1;

        public void CheckAndExecute(DamageInfo di)
        {
            if (!di.Target.IsDead)
                return;
            
            _animator.SetTrigger(AnimatorParameters.DieHash);
            Destroy(transform.parent.gameObject, _timeout);
        }
    }
}
