using UnityEngine;

public abstract class AttackSO : ScriptableObject
{
    [field: SerializeField] public virtual float ReloadTime { get; private set; } = 0.8f;
    [field: SerializeField] public virtual float SpeedMultiplier { get; private set; } = 0.3f;
}
