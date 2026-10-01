using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class HealthResource : ValueResource
{
    [SerializeField] PlayerCombatData _combatData;
    public Collider2D Hurtbox { get; private set; }
    float _invincibilityTimer;

    public event Action<DamageInfo> HpChangeApplied;
    public event Action<DamageInfo> OnHpChange;
    public event Action<DamageInfo> Died;
    public event Action<DamageInfo> OnMax;

    protected override void Awake()
    {
        base.Awake();

        Hurtbox = GetComponent<Collider2D>();
    }

    public bool IsInvincible => _invincibilityTimer > 0f;
    public bool IsDead => CurrentValue <= 0;

    public void ApplyDamage(DamageInfo damageInfo)
    {
        if (IsInvincible)
            return;

        if (_combatData) damageInfo.Amount = Mathf.RoundToInt(damageInfo.Amount / _combatData.Protection);
        OnHpChange?.Invoke(damageInfo);
        int applied = Consume(damageInfo.Amount);
        if (applied == 0)
            return;

        damageInfo.Amount = applied;
        HpChangeApplied?.Invoke(damageInfo);

        if (applied != 0)
        {
            if (IsDead)
                Died?.Invoke(damageInfo);
            if (CurrentValue == MaxValue)
                OnMax?.Invoke(damageInfo);
        }
    }

    public void GrantInvincibility(float time)
        => _invincibilityTimer = Mathf.Max(_invincibilityTimer, time);

    private void Update()
    {
        if (_invincibilityTimer > 0f)
            _invincibilityTimer -= Time.deltaTime;
    }
}
