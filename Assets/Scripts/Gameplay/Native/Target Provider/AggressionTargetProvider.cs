using UnityEngine;

public class AggressionTargetProvider : TargetProvider
{
    private void OnEnable()
    {
        //TODO
        //healthComponent.OnValueChanged += BecomeAgressive;
    }

    private void OnDisable()
    {
        //healthComponent.OnValueChanged -= BecomeAgressive;
    }

    public void BecomeAgressive(GameObject gameObject, int _)
    {
        CurrentTarget = gameObject;
    }

    public override TargetProvider Init(TargetProviderSO other)
    {
        return this;
    }
}
