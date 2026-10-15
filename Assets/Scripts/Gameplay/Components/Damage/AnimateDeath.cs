using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class AnimateDeath : MonoBehaviour
    {
        [SerializeField] EntityAnimationController _animationController;
        [SerializeField] Health _health;

        private void Awake()
        {
            if (_animationController == null || _health == null)
                throw new MissingReferenceException();
        }

        void OnEnable() => _health.Died += Animate;
        void OnDisable() => _health.Died -= Animate;
        void Animate(DamageInfo _)
        {
            _animationController.PlayDeath();
        }

        public void Destroy() => Destroy(transform.parent.gameObject);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animationController == null)
                _animationController = GetComponentInChildren<EntityAnimationController>();
        }
#endif
    }
}
