using Recalled.Systems.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.UI.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/SpeakerMap")]
    public class SpeakerRegistry : ScriptableObject
    {
        [SerializeField] Dictionary<SpeakerSO, Sprite> _speakerToSpriteMap;

        public IReadOnlyDictionary<SpeakerSO, Sprite> SpriteMap => _speakerToSpriteMap;

        private void OnEnable()
        {
            if (_speakerToSpriteMap.Values.Contains(null))
                Debug.LogWarning($"{name} contains null for some key. Please fix it", this);
        }
    }
}
