using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

namespace Recalled.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Emotions/Registry")]
    public class EmotionRegistry : ScriptableObject
    {
        [SerializeField] Dictionary<EmotionSO, Sprite> _emotionToSpriteMap;

        public IReadOnlyDictionary<EmotionSO, Sprite> SpriteMap => _emotionToSpriteMap;

        private void OnEnable()
        {
            if (_emotionToSpriteMap.Values.Contains(null))
                Debug.LogWarning($"{name} contains null for some key. Please fix it", this);
        }
    }
}
