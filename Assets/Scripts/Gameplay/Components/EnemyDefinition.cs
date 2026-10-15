using UnityEngine;

namespace Recalled.Gameplay
{
    [CreateAssetMenu(menuName = "Recalled/Enemies/Definition")]
    public sealed class EnemyDefinition : ScriptableObject
    {
        [Header("Composition (consumed by the editor-time prefab builder)")]
        [Tooltip("Logic-only prefab: collider, health, AI, attack, and an empty child with a VisualRoot component.")]
        [SerializeField] GameObject _basePrefab;

        [Tooltip("Renderer hierarchy with its Animator — the Aseprite-imported prefab or a variant of it.")]
        [SerializeField] GameObject _visualPrefab;

        public GameObject BasePrefab => _basePrefab;
        public GameObject VisualPrefab => _visualPrefab;
    }
}