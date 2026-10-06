using UnityEngine;
using UnityEngine.Audio;

namespace Recalled.Gameplay
{
    public readonly struct AudioClipRequest
    {
        public readonly AudioClip Clip;
        public readonly AudioMixerGroup MixerGroup;
        public readonly Vector2 Position;

        public AudioClipRequest(AudioClip clip, AudioMixerGroup group, Vector2 position)
        {
            Clip = clip;
            MixerGroup = group;
            Position = position;
        }
    }
}
