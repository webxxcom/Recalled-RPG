using Recalled.Gameplay.Combat;
using UnityEngine;

namespace Recalled.Gameplay
{
    internal sealed class DamageKnockbackApplier : MonoBehaviour
    {
        public void Apply(in DamageInfo di)
        {
            di.Target.GetComponent<ExternalVelocity>().Add(di.Direction * di.KnockbackPower);
        }
    }
}
