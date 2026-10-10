using UnityEngine;
using UnityEngine.Audio;

namespace Recalled.Core
{
    [CreateAssetMenu(menuName = "Sounds/Asset")]
    public class SoundCue : ScriptableObject
    {
        [SerializeField] AudioClip _clip;
        [SerializeField] AudioMixerGroup _mixerGroup;

        public AudioClip Clip => _clip;
        public AudioMixerGroup AudioMixerGroup => _mixerGroup;
    }
}
