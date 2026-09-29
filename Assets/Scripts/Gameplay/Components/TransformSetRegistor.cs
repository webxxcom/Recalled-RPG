using UnityEngine;

public class TransformSetRegistor : AutoSetRegistor<Transform>
{
    protected override Transform Data => transform;
}
