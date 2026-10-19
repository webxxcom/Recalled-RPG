using UnityEngine;

public abstract class MovementStrategy
{
    public MovementStrategy(MovementStrategySO other, GameObject root) { Init(other, root); }
    public abstract void Init(MovementStrategySO other, GameObject root);
    public abstract Vector2 GetDirection(GameObject origin, GameObject target);
}
