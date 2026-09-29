using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatorVFX : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;

    private void Awake()
        => _spriteRenderer = GetComponent<SpriteRenderer>();

    private void Start()
    {
        var angleZ = Mathf.Abs(transform.rotation.eulerAngles.z);
        if (angleZ < 270 && angleZ > 90)
            _spriteRenderer.flipY = true;
    }
}
