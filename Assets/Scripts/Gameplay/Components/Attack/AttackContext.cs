using System;
using UnityEngine;

namespace Recalled.Gameplay.Combat
{
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
}
