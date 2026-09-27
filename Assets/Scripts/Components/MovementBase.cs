using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovementBase : MonoBehaviour
{
    [SerializeField] float _walkingSpeed;

    public Vector2 LastMovement { get; private set; }
    public bool IsWalking => MovementIntention != Vector2.zero;
    public Vector2 FacingDirection => MovementIntention != Vector2.zero ? MovementIntention : LastMovement;
    public float CurrentSpeed => _rigidbody2D.linearVelocity.magnitude;

    [Header("Uses")]
    [SerializeField] ExternalVelocity _externalVelocity;
    [SerializeField] AnimationController _animationController;

    Rigidbody2D _rigidbody2D;
    readonly SpeedAggregator _speedAggregator = new();
    Vector2 _movementIntention;

    public Vector2 MovementIntention
    {
        get => _movementIntention;
        protected set
        {
            if (_movementIntention != Vector2.zero)
                LastMovement = _movementIntention;

            var _prevMov = _movementIntention;
            if (_prevMov == Vector2.zero && value != Vector2.zero)
                OnMovementStarted?.Invoke();
            else if (_prevMov != Vector2.zero && value == Vector2.zero)
                OnMovementStopped?.Invoke();

            _movementIntention = value;
            if (value != Vector2.zero)
                OnMovement?.Invoke();
        }
    }

    public event Action OnMovementStarted;
    public event Action OnMovementStopped;
    public event Action OnMovement;

    private void OnDisable()
    {
        MovementIntention = Vector2.zero;
    }

    protected abstract Vector2 GetMovementIntention();
    public Vector2 GetFinalMovement()
    {
        float speedCoeficient = _speedAggregator.Get();
        Vector2 externalVelocity = _externalVelocity != null ? _externalVelocity.TickAndGet(Time.fixedDeltaTime) : Vector2.zero;
        Vector2 finalMovement = GetMovementIntention();
        return (speedCoeficient * _walkingSpeed * finalMovement) + externalVelocity;
    }

    private void FixedUpdate()
    {
        Vector2 finalMovement = GetFinalMovement();

        if (finalMovement != Vector2.zero)
            _rigidbody2D.linearVelocity = finalMovement;
        else
            _rigidbody2D.linearVelocity *= 0.9f;

        _animationController.MoveAnimation(FacingDirection,
            _rigidbody2D.linearVelocity.magnitude / 4f);
    }

    public void AddSpeedCoef(float coef) => _speedAggregator.Add(coef);
    public void RemoveSpeedCoef(float coef) => _speedAggregator.Remove(coef);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_rigidbody2D == null)
            _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_animationController == null)
            _animationController = GetComponentInChildren<AnimationController>();
    }
#endif
}
