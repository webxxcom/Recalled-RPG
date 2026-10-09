using UnityEngine;

namespace Recalled.Systems.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Emotions/Definition")]
    public class EmotionSO : ScriptableObject
    {
        [SerializeField] string _name;

        public string Name => _name;
    }
}