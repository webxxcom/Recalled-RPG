using UnityEngine;

[CreateAssetMenu(menuName = "ApplyAttack / Projectile ApplyAttack Value")]
public class ProjectileAttackDataSO : AttackSO
{
    [field: SerializeField] public GameObject ProjectilePrefab { get; private set; }
    [field: SerializeField] public float NormalizedSpawnPoint { get; private set; }
    [field: SerializeField] public float Range { get; private set; }
}
