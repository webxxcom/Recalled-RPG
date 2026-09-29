using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityMovement : MovementBase
{
    [Header("Config")]
    [SerializeField] MovementAIConfig _movementAIConfig;

    MovementStrategy _idleMovementStrategy;
    readonly List<MovementStrategy> _movementStrategies = new();
    readonly List<TargetProvider> _targetProviders = new();

    void Awake()
    {
        _idleMovementStrategy = _movementAIConfig.IdleMovementStrategy.CreateInstance(gameObject);

        foreach (var item in _movementAIConfig.MovementStrategies)
            _movementStrategies.Add(item.CreateInstance(gameObject));
        foreach (var item in _movementAIConfig.TargetProviders)
            _targetProviders.Add(item.CreateInstance());
    }

    protected override Vector2 GetMovementIntention()
    {
        GameObject target = _targetProviders
            .FirstOrDefault(e => e.HasTarget)?.CurrentTarget;

        Vector2 dir = Vector2.zero;
        if (target != null)
        {
            foreach (var movementStrategy in _movementStrategies)
            {
                dir = movementStrategy.GetDirection(gameObject, target);

                if (dir != Vector2.zero)
                    break;
            }
        }
        else
            dir = _idleMovementStrategy.GetDirection(gameObject, target);

        MovementIntention = dir;
        return MovementIntention;
    }
}
