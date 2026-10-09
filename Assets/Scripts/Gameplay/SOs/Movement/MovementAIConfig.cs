using UnityEngine;

[CreateAssetMenu(menuName = "Movement/MovementAIConfig")]
public class MovementAIConfig : ScriptableObject
{
    [field: SerializeField] public MovementStrategySO IdleMovementStrategy { get; private set; }
    [field: SerializeField] public MovementStrategySO[] MovementStrategies { get; private set; }
    [field: SerializeField] public TargetProviderSO[] TargetProviders { get; private set; }
}
