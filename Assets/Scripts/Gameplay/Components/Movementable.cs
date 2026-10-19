using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ExternalVelocity))]
public abstract class Movementable : MonoBehaviour
{
    [SerializeField] float _walkingSpeed;

    public bool IsWalking => MovementIntention != Vector2.zero;
    public Vector2 FacingDirection => MovementIntention != Vector2.zero ? MovementIntention : _lastMovement;
    public float CurrentSpeed => _rigidbody2D.linearVelocity.magnitude;
    Vector2 _movementIntention;
    Vector2 _lastMovement;

    Rigidbody2D _rigidbody2D;
    ExternalVelocity _externalVelocity;
    EntityAnimationController _animationController;
    readonly SpeedAggregator _speedAggregator = new();

    public Vector2 MovementIntention
    {
        get => _movementIntention;
        protected set
        {
            if (_movementIntention != Vector2.zero)
                _lastMovement = _movementIntention;

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

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _externalVelocity = GetComponent<ExternalVelocity>();
        _animationController = GetComponentInChildren<EntityAnimationController>();
    }

    private void OnDisable()
    {
        MovementIntention = Vector2.zero;
    }

    protected virtual Vector2 GetMovementIntention() => MovementIntention;
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

    void FullStop()
    {
        MovementIntention = Vector2.zero;
        _rigidbody2D.linearVelocity = Vector2.zero;
        _animationController.MoveAnimation(FacingDirection, 0);
    }

    public void AddSpeedCoef(float coef) => _speedAggregator.Add(coef);
    public void RemoveSpeedCoef(float coef) => _speedAggregator.Remove(coef);

    private void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0))
            FullStop();
    }
}
