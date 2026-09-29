using UnityEngine;

public class DashSound : EntitySound
{
    [SerializeField] AudioClip _sound;
    Dash _dash;

    private void OnEnable()
        => _dash.OnDash += PlaySound;
    private void OnDisable()
        => _dash.OnDash -= PlaySound;
    void PlaySound()
        => _audioSource.PlayOneShot(_sound);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_dash == null)
            _dash = GetComponentInParent<Dash>();
    }
#endif
}
