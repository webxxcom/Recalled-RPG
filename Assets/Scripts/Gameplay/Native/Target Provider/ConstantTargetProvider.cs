using System;
using UnityEngine;

public class ConstantTargetProvider : TargetProvider
{
    public override TargetProvider Init(TargetProviderSO other)
    {
        ConstantTargetProviderSO constantTargetProviderSO = other as ConstantTargetProviderSO;

        CurrentTarget = constantTargetProviderSO.Target;

        return this;
    }
}
