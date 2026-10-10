using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ExternalVelocity))]
[RequireComponent(typeof(SprintingResource))]
public class Dash : MonoBehaviour
{
    [SerializeField] float _consumeSprntNorm;
    [SerializeField] float _force;
    [SerializeField] float _invincibilityDuration;
    [SerializeField] GameObject _vfx;

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
