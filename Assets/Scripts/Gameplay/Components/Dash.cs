using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ExternalVelocity), typeof(SprintingResource))]
    public sealed class Dash : MonoBehaviour
    {
        [SerializeField] float _consumeSprntNorm;
        [SerializeField] float _force;
        [SerializeField] float _invincibilityDuration;
        [SerializeField] AnimatorVFX _vfx;

        public event Action OnDash;

        HealthResource _health;
        ExternalVelocity _externalVelocity;
        SprintingResource _sprinting;

        private void Awake()
        {
            _externalVelocity = GetComponent<ExternalVelocity>();
            _sprinting = GetComponent<SprintingResource>();
            _health = GetComponentInChildren<HealthResource>();
        }

        float TargetConsumedSprinting => _sprinting.MaxValue * _consumeSprntNorm;
        public bool TryDash(Vector2 direction)
        {
            float targetConsumed = TargetConsumedSprinting;
            if (_sprinting.CurrentValue < targetConsumed)
                return false;

            OnDash?.Invoke();
            _sprinting.Consume(Mathf.RoundToInt(targetConsumed));
            _externalVelocity.Add(direction * _force);
            _health.GrantInvincibility(_invincibilityDuration);
            Instantiate(_vfx, transform.position, Quaternion.FromToRotation(Vector2.right, direction));
            return true;
        }
    }
}
