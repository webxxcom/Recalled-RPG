using UnityEngine;

public class HealthSound : EntitySound
{
    [SerializeField] AudioClip _hurtSound;
    [SerializeField] AudioClip _healingSound;
    [SerializeField] AudioClip _deathSound;
    [SerializeField] HealthResource _healthProvider;

    void OnEnable()
    {
        _healthProvider.OnMinValue += HandleDeathSound;
        _healthProvider.OnValueChanged += HandleHurtHealingSound;
    }

    void OnDisable()
    {
        _healthProvider.OnMinValue += HandleDeathSound;
        _healthProvider.OnValueChanged += HandleHurtHealingSound;
    }

    public void HandleDeathSound(int _)
    {
        _audioSource.PlayOneShot(_deathSound);
    }

    public void HandleHurtHealingSound(int oldVal, int newVal)
    {
        int amount = oldVal - newVal;
        if (amount > 0)
            _audioSource.PlayOneShot(_hurtSound);
        else if (amount < 0)
            _audioSource.PlayOneShot(_healingSound);
    }
}
