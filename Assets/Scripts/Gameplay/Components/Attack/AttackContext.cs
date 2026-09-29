using UnityEngine;

public class AttackContext
{
    public GameObject Target { get; private set; }

    public AttackContext(GameObject target)
    {
        Target = target;
    }
}
