using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Recalled.Gameplay
{
    public class AudioGroup : MonoBehaviour
    {
        [SerializeField] Dictionary<AudioMixerGroup, AudioSource> _mixerGroupToAudioSource;
        [SerializeField] AudioClipRequestGameEvent AudioClipRequested;

        private void OnEnable()
        {
            AudioClipRequested.AddListener(HandleClipRequest);
        }

        private void OnDisable()
        {
            AudioClipRequested.RemoveListener(HandleClipRequest);
        }

        void PlayRequestWith(in AudioClipRequest request, AudioSource template)
        {
            var audioSource = Instantiate(template, request.Position, Quaternion.identity);
            audioSource.clip = request.Clip;
            audioSource.outputAudioMixerGroup = request.MixerGroup;
            audioSource.Play();

            Destroy(audioSource.gameObject, request.Clip.length);
        }

        void HandleClipRequest(AudioClipRequest request)
        {
            if (_mixerGroupToAudioSource.TryGetValue(request.MixerGroup, out var res))
                PlayRequestWith(request, res);
            else
                Debug.LogError($"Didn't find the AudioSource for {request.MixerGroup} mixer group");
        }
    }
}
