using UnityEngine;

public abstract class TargetProvider
{
    public bool HasTarget => CurrentTarget != null;

    public GameObject CurrentTarget { get; protected set; }

    public abstract TargetProvider Init(TargetProviderSO other);
}
