using System.Collections;
using UnityEngine;

public class DamageBlinking : HealthReactor
{
    [SerializeField] float _duration;
    [SerializeField] float _blinkInterval;
    [SerializeField] SpriteRendererGroup _spriteRendererGroup;

    protected override void OnHpChangeApplied(DamageInfo di)
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
