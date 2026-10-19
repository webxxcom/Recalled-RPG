using UnityEngine;

public class WanderingMovement : MovementStrategy
{
    private static readonly Vector2[] Directions =
        {
        Vector2.right,
        Vector2.left,
        Vector2.down,
        Vector2.up
    };

    Vector2 IntermediateDirection
    {
        get => _intermediateDirection;
        set
        {
            _intermediateDirection = value;
            _elapsedSinceChangingDirection = 0;
        }
    }
    float _elapsedSinceChangingDirection = 0;
    Vector2 _intermediateDirection;
    bool _isIdle;
    float _currentStateDuration;
    Collider2D _collider2D;

    bool ShouldChangeDirection => _elapsedSinceChangingDirection >= _currentStateDuration;
    public override Vector2 GetDirection(GameObject _, GameObject _1)
    {
        if (ShouldChangeDirection)
            AdvanceState();
        if (_collider2D.IsTouchingLayers() && _elapsedSinceChangingDirection > 0.6f)
            _intermediateDirection *= -1;

        _elapsedSinceChangingDirection += Time.deltaTime;
        return IntermediateDirection;
    }

    void FindNewDirection()
    {
        Vector2 newDirection;

        do
        {
            newDirection = Directions[Random.Range(0, 4)];
        } while (newDirection == IntermediateDirection);

        IntermediateDirection = newDirection;
    }

    void AdvanceState()
    {
        _currentStateDuration = Random.Range(0.6f, 2.3f);
        _isIdle = !_isIdle;

        if (!_isIdle)
            FindNewDirection();
        else
            IntermediateDirection = Vector2.zero;
    }

    public override void Init(MovementStrategySO other, GameObject root)
    {
        _collider2D = root.GetComponent<Collider2D>();
    }

    public WanderingMovement(MovementStrategySO other, GameObject root) : base(other, root) { }
}
