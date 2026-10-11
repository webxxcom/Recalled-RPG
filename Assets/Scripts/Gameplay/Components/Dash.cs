using System;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ExternalVelocity))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Sprint))]
    public sealed class Dash : MonoBehaviour
    {
        [SerializeField] int _consume;
        [SerializeField] float _force;
        [SerializeField] float _invincibilityDuration = 0.3f;
        [SerializeField] float _reload = 0.8f;
        [SerializeField] AnimatorVFX _vfx;
        [SerializeField] UnityEvent _dashed;

        Health _health;
        ExternalVelocity _externalVelocity;
        Sprint _sprint;
        float _lastDashTimestamp;

        private void Awake()
        {
            _externalVelocity = GetComponent<ExternalVelocity>();
            _sprint = GetComponent<Sprint>();
            _health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            _lastDashTimestamp = -_reload;
        }

        public bool TryDash(Vector2 direction)
        {
            if (!_sprint.AskFor(_consume) || _lastDashTimestamp + _reload > Time.time)
                return false;

            _dashed.Invoke();
            _lastDashTimestamp = Time.time;
            _externalVelocity.Add(direction * _force);
            _health.GrantInvincibility(_invincibilityDuration);
            Instantiate(_vfx, transform.position, Quaternion.FromToRotation(Vector2.right, direction));
            return true;
        }
    }
}
