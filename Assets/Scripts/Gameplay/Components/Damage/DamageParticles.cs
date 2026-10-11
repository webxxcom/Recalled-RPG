using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class DamageParticles : MonoBehaviour
    {
        [SerializeField] ParticleSystem _particles;
        [SerializeField] Health _health;

        void OnEnable() => _health.HpChanged += Spawn;
        void OnDisable() => _health.HpChanged -= Spawn;

        public void Spawn(DamageInfo di)
        {
            Instantiate(_particles, di.Target.Center, Quaternion.FromToRotation(Vector3.right, di.Direction));
        }
    }
}
