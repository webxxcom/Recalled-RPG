using UnityEngine;

namespace Recalled.UI
{
    public class IdleSpriteAnimator : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;

        GraphicAnimator _animator;

        private void OnEnable()
        {
            _spriteRenderer.enabled = true;
        }
        private void OnDisable()
        {
            _spriteRenderer.enabled = false;
        }

        public void Play(SpriteSequence sequence)
        {
            _animator = new(sequence);
        }

        public void Stop()
        {
            _animator = null;
        }

        private void Update()
        {
            if (_animator == null) return;

            Sprite sprite = _animator.Update(false);
            if (sprite != _spriteRenderer.sprite)
                _spriteRenderer.sprite = sprite;
        }
    }
}
