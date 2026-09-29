using UnityEngine;

public abstract class TargetProviderSO : ScriptableObject
{
    public abstract TargetProvider CreateInstance();
}
