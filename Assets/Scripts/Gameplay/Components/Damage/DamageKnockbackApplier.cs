using UnityEngine;

namespace Recalled.Gameplay
{
    public class DamageKnockbackApplier : MonoBehaviour
    {
        public void Apply(in DamageInfo di)
        {
            di.Target.GetComponent<ExternalVelocity>().Add(di.Direction * di.KnockbackPower);
        }
    }
}
