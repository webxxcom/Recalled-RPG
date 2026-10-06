using UnityEngine;
using UnityEngine.Audio;

namespace Recalled.Gameplay
{
    public class ReactorSound : MonoBehaviour, IReactor
    {
        [SerializeField] AudioClip _clip;
        [SerializeField] AudioMixerGroup _mixerGroup;

        [Header("Raises")]
        [SerializeField] AudioClipRequestGameEvent AudioClipRequestChannel;

        public void React()
        {
            AudioClipRequestChannel.Invoke(new(_clip, _mixerGroup, transform.position));
        }
    }
}
