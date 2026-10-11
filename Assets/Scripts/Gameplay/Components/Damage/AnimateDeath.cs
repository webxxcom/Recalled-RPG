using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class AnimateDeath : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] Health _health;

        private void Awake()
        {
            if (_animator == null || _health == null)
                throw new MissingReferenceException();
        }

        void OnEnable() => _health.Died += Animate;
        void OnDisable() => _health.Died -= Animate;
        void Animate(DamageInfo _)
        {
            _animator.SetTrigger(AnimatorParameters.DieHash);
        }

        public void Destroy() => Destroy(transform.parent.gameObject);
    }
}
