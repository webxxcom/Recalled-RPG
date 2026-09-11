using UnityEngine;

public class PlayerController : EntityController
{
    bool _isArmed;
    public bool IsArmed
    {
        get => _isArmed;
        private set
        {
            _isArmed = value;
            Animator.SetBool(AnimatorParameters.IsArmedHash, value);
        }
    }

    [Header("Broadcasts to")]
    [SerializeField] DamageInfoGameEvent OnHpChangedChannel;
    [SerializeField] DamageInfoGameEvent OnDeathChannel;

    HealthResource _healthProvider;

    protected override void Awake()
    {
        base.Awake();

        _healthProvider = GetComponentInChildren<HealthResource>();

        IsArmed = true;
    }

    void OnEnable()
    {
        _healthProvider.OnHpChangeApplied += HandleOnHpChangedGameEvent;
        _healthProvider.OnDeath += HandleOnDeathGameEvent;
    }

    void OnDisable()
    {
        _healthProvider.OnHpChangeApplied -= HandleOnHpChangedGameEvent;
        _healthProvider.OnDeath -= HandleOnDeathGameEvent;
    }

    void HandleOnHpChangedGameEvent(DamageInfo damageInfo) => OnHpChangedChannel.Raise(damageInfo);
    void HandleOnDeathGameEvent(DamageInfo damageInfo) => OnDeathChannel.Raise(damageInfo);
}
