using UnityEngine;

public class ChaseMovement : MovementStrategy
{
    [SerializeField] float _distanceToTarget;
    [SerializeField] float _delta;

    public override Vector2 GetDirection(GameObject origin, GameObject target)
    {
        Vector2 diff = target.transform.position - origin.transform.position;


        bool flee = !Mathf.Approximately(_delta, 0);
        float maxDist = _distanceToTarget + _delta;
        float minDist = _distanceToTarget - _delta;
        float sqrMax = maxDist * maxDist;
        float sqrMin = minDist * minDist;

        if (diff.sqrMagnitude > sqrMax && diff.sqrMagnitude > sqrMin)
            return diff.normalized;
        else if (diff.sqrMagnitude < sqrMin && diff.sqrMagnitude < sqrMax && flee)
            return diff.normalized * -1;

        return Vector2.zero;
    }

    public override void Init(MovementStrategySO other, GameObject root)
    {
        ChaseMovementSO chaseMovementSO = other as ChaseMovementSO;

        _distanceToTarget = chaseMovementSO.MinDistanceToTarget;
        _delta = chaseMovementSO.Delta;
    }
    public ChaseMovement(MovementStrategySO other, GameObject root) : base(other, root) { }
}
