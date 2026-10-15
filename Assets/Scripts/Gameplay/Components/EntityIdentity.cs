using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    public sealed class EntityIdentity : MonoBehaviour
    {
        [SerializeField] EnemyDefinition _definition;

        public EnemyDefinition Definition => _definition;
        public const string DefinitionFieldName = nameof(_definition);
    }
}
