using UnityEngine;

public class TrainingDummyController : EntityController
{
    private static readonly int DamageHardHash = Animator.StringToHash("HurtHard");
    private static readonly int DamageMidHash = Animator.StringToHash("HurtMid");
    private static readonly int DamageLightHash = Animator.StringToHash("HurtLight");

    [SerializeField] HealthResource _health;
    [SerializeField] Animator _animator;

    void HpChanged(DamageInfo damageInfo)
    {
        if (damageInfo.Amount > 0)
        {
            if (damageInfo.Amount <= 10)
                _animator.SetTrigger(DamageLightHash);
            else if (damageInfo.Amount <= 25)
                _animator.SetTrigger(DamageMidHash);
            else
                _animator.SetTrigger(DamageHardHash);
        }
    }

    private void OnEnable() => _health.OnHpChange += HpChanged;
    private void OnDisable() => _health.OnHpChange -= HpChanged;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_health == null)
            _health = GetComponentInChildren<HealthResource>();
        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();
    }
#endif
}
