using System;
using UnityEngine;
[DisallowMultipleComponent]
[RequireComponent(typeof(ExternalVelocity))]
[RequireComponent(typeof(SprintingResource))]

[RequireComponent(typeof(MovementBase))]
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
    MovementBase _movementBase;

    private void Awake()
    {
        _externalVelocity = GetComponent<ExternalVelocity>();
        _sprinting = GetComponent<SprintingResource>();
        _movementBase = GetComponent<MovementBase>();
        _health = GetComponentInChildren<HealthResource>();
    }

    float TargetConsumedSprinting => _sprinting.MaxValue * _consumeSprntNorm;
    public bool TryDash(Vector2 direction)
    {
        if (_sprinting.CurrentValue < TargetConsumedSprinting)
            return false;

        OnDash?.Invoke();
        _sprinting.Consume(Mathf.RoundToInt(TargetConsumedSprinting));
        _externalVelocity.Add(direction * _force);
        _health.GrantInvincibility(_invincibilityDuration);
        Instantiate(_vfx, transform.position, Quaternion.FromToRotation(Vector2.right, _movementBase.FacingDirection));
        return true;
    }
}
