using UnityEngine;

namespace Recalled.Gameplay
{
    public class DamageKnockbackApplier : MonoBehaviour
    {
        public void Apply(DamageInfo di)
        {
            di.Target.transform.parent
                .GetComponent<ExternalVelocity>().Add(di.Direction * di.KnockbackPower);
        }
    }
}
