using UnityEngine;

namespace Recalled.Gameplay
{
    public class IdleSpriteAnimator : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] SpriteSequence _startAnimation;

        GraphicAnimator _animator;

        private void OnEnable()
        {
            _spriteRenderer.enabled = true;

            if (_startAnimation) Play(_startAnimation);
        }
        private void OnDisable()
        {
            _spriteRenderer.enabled = false;
        }

        public void Play(SpriteSequence sequence)
        {
            _animator = new(sequence);
        }

        public void Play()
        {
            _animator = new(_startAnimation);
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
