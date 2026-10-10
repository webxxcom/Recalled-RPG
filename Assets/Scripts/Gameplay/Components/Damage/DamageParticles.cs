using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class DamageParticles : MonoBehaviour
    {
        [SerializeField] ParticleSystem _particles;

        public void Spawn(DamageInfo di)
        {
            Instantiate(_particles, di.Target.Hurtbox.bounds.center, Quaternion.FromToRotation(Vector3.right, di.Direction));
        }
    }
}
