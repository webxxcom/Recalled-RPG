using Recalled.Core;
using Recalled.Gameplay.Combat;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public sealed class Health : MonoBehaviour
    {

        [SerializeField] ValueProviderConfig _config;
        [SerializeField] Collider2D _hurtbox;

        [Tooltip("Optional Runtime Variable, mostly for player and bosses")]
        [SerializeField] HealthValueVariable _healthVariable;

        public Vector2 Center => _hurtbox.bounds.center;
        public Collider2D Hurtbox => _hurtbox;
        public bool IsInvincible => _invincibilityTimestamp > Time.time;
        public bool IsFull => _valueResource.IsFull;
        public bool IsDead => _valueResource.Current <= _valueResource.MinValue;
        public int Current => _valueResource.Current;
        public int MaxValue => _valueResource.MaxValue;
        float _invincibilityTimestamp;
        ValueResource _valueResource;

        public event Action<DamageInfo> HpChanged;
        [SerializeField] UnityEvent<DamageInfo> _hpChangedEvent;
        [SerializeField] UnityEvent<DamageInfo> _diedEvent;
        public event Action<DamageInfo> Died;
        public event Action OnMax;

        void Awake()
        {
            _valueResource = new(_config);

            if (_healthVariable != null) _healthVariable.Value = new(_valueResource.Current, _valueResource.MaxValue);
        }

        public void ApplyDamage(DamageInfo damageInfo)
        {
            if (IsInvincible)
                return;

            int applied = _valueResource.Consume(damageInfo.Amount);
            if (_healthVariable != null)
                _healthVariable.Value = new(_valueResource.Current, _valueResource.MaxValue);

            HpChanged?.Invoke(damageInfo);
            _hpChangedEvent?.Invoke(damageInfo);

            if (applied != 0)
            {
                if (IsDead)
                {
                    Died?.Invoke(damageInfo);
                    _diedEvent.Invoke(damageInfo);
                }
            }
        }

        public void Heal(int healInfo)
        {
            int applied = _valueResource.Replenish(healInfo);
            if (_healthVariable != null)
                _healthVariable.Value = new(_valueResource.Current, _valueResource.MaxValue);

            if (applied != 0)
            {
                if (IsFull)
                    OnMax?.Invoke();
            }
        }

        public void GrantInvincibility(float time)
        {
            if (time < 0)
                throw new ArgumentException();

            _invincibilityTimestamp = Mathf.Max(Time.time + time, _invincibilityTimestamp);
        }
    }
}
