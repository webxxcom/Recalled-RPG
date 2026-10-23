using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class EntityAnimationController : MonoBehaviour
    {
        [SerializeField] bool _isXFlippable;

        public bool FlippedX { get; private set; }

        Animator _animator;
        SpriteRendererGroup _spriteRendererGroup;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _spriteRendererGroup = GetComponentInChildren<SpriteRendererGroup>();
        }

        public void MoveAnimation(Vector2 direction, float speed)
        {
            _animator.SetFloat(AnimatorParameters.MoveXHash, direction.x);
            _animator.SetFloat(AnimatorParameters.MoveYHash, direction.y);
            _animator.SetFloat(AnimatorParameters.SpeedHash, !Mathf.Approximately(speed, 0) ? speed : 0f);

            if (_isXFlippable)
            {
                if (direction.x > float.Epsilon)
                {
                    _spriteRendererGroup.SetFlipX(false);
                    FlippedX = false;
                }
                else if (direction.y < -float.Epsilon)
                {
                    _spriteRendererGroup.SetFlipX(true);
                    FlippedX = true;
                }
            }
        }

        public void PlayDeath()
        {
            _animator.SetTrigger(AnimatorParameters.DieHash);
        }

        public void PlayAttack(int hash)
        {
            _animator.SetTrigger(hash);
        }
    }
}
