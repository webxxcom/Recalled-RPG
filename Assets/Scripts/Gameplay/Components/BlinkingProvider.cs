using System.Collections;
using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class BlinkingProvider : MonoBehaviour
    {
        [SerializeField] float _duration;
        [SerializeField] float _blinkInterval;
        [SerializeField] SpriteRendererGroup _spriteRendererGroup;

        public void Blink()
        {
            StopAllCoroutines();
            StartCoroutine(BlinkCoroutine());
        }

        IEnumerator BlinkCoroutine()
        {
            float elapsed = 0;
            while (elapsed < _duration)
            {
                _spriteRendererGroup.SetAlpha(0);
                yield return new WaitForSeconds(_blinkInterval);

                _spriteRendererGroup.SetAlpha(1);
                yield return new WaitForSeconds(_blinkInterval);

                elapsed += _blinkInterval * 2;
            }
            _spriteRendererGroup.SetAlpha(1);
        }
    }
}
