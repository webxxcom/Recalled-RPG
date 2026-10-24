using UnityEngine;

namespace Recalled.Gameplay
{
    [CreateAssetMenu(menuName = "ApplyAttack/Projectile Attack")]
    public class ProjectileAttackDataSO : AttackSO
    {
        [field: SerializeField] public Projectile ProjectilePrefab { get; private set; }
        [field: SerializeField] public float NormalizedSpawnPoint { get; private set; }
        [field: SerializeField] public float Range { get; private set; }
    }
}
