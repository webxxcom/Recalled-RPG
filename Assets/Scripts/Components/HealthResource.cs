using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class HealthResource : ValueResource
{
    [SerializeField] PlayerCombatData _combatData;
    public Collider2D Hurtbox { get; private set; }
    public bool IsInvincible => _invincibilityTimer > 0f;
    float _invincibilityTimer;

    public event Action<DamageInfo> OnHpChangeApplied;
    public event Action<DamageInfo> OnHpChange;
    public event Action<DamageInfo> OnDeath;
    public event Action<DamageInfo> OnMax;

    protected override void Awake()
    {
        base.Awake();

        Hurtbox = GetComponent<Collider2D>();
    }

    public bool IsDead => CurrentValue <= 0;

    public void ApplyDamage(DamageInfo damageInfo)
    {
        if (IsInvincible)
            return;

        if (_combatData) damageInfo.Amount = Mathf.RoundToInt(damageInfo.Amount / _combatData.Protection);
        OnHpChange?.Invoke(damageInfo);
        int applied = Replenish(-damageInfo.Amount);
        if (applied == 0)
            return;

        damageInfo.Amount = applied;
        OnHpChangeApplied?.Invoke(damageInfo);

        if (CurrentValue == 0)
            OnDeath?.Invoke(damageInfo);
        if (CurrentValue == MaxValue)
            OnMax?.Invoke(damageInfo);
    }

    public void GrantInvincibility(float time)
        => _invincibilityTimer = Mathf.Max(_invincibilityTimer, time);

    private void Update()
    {
        if (_invincibilityTimer > 0f)
            _invincibilityTimer -= Time.deltaTime;
    }
}
