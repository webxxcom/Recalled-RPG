using UnityEngine;

namespace Recalled.Gameplay
{
    [CreateAssetMenu(menuName = "Recalled/Enemies/Definition")]
    public sealed class EnemyDefinition : ScriptableObject
    {
        [SerializeField] GameObject _basePrefab;
        [SerializeField] GameObject _visualPrefab;
        [SerializeField] Movement.State _movementState;

        public GameObject BasePrefab => _basePrefab;
        public GameObject VisualPrefab => _visualPrefab;
        public Movement.State MovementState => _movementState;
    }
}