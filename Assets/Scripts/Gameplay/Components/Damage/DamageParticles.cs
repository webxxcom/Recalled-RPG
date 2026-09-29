using UnityEngine;

public class DamageParticles : HealthReactor
{
    [SerializeField] ParticleSystem _particles;

    protected override void OnHpChangeApplied(DamageInfo di)
        => Instantiate(_particles, _health.Hurtbox.bounds.center, Quaternion.FromToRotation(Vector3.right, di.Direction));
}
