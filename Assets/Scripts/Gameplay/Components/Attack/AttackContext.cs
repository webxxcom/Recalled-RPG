using System;
using UnityEngine;

public readonly struct AttackContext
{
    public readonly GameObject Target;

    public AttackContext(GameObject target)
    {
        if (target == null)
            throw new ArgumentNullException();

        Target = target;
    }
}
