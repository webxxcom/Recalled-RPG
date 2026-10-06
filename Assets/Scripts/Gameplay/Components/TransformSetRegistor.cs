using UnityEngine;

namespace Recalled.Core
{
    public sealed class TransformSetRegistor : AutoSetRegistor<Transform>
    {
        protected override Transform Data => transform;
    }
}
