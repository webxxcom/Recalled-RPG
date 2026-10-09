using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Chasing")]
public class ChaseMovementSO : MovementStrategySO
{
    [field: SerializeField] public float MinDistanceToTarget { get; private set; }
    [field: SerializeField] public float Delta { get; private set; }

    public override MovementStrategy CreateInstance(GameObject root)
        => new ChaseMovement(this, root);
}
