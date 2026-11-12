using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Recalled.Gameplay
{
    public sealed class AudioGroup : MonoBehaviour
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

        void PlayRequestWith(AudioClipRequest request, AudioSource template)
        {
            var audioSource = Instantiate(template, request.Position, Quaternion.identity);
            audioSource.clip = request.AudioAsset.Clip;
            audioSource.Play();

            audioSource.gameObject.AddComponent<AutoDestroyAfter>()
                .Init(true, request.AudioAsset.Clip.length + 0.1f);
        }

        void HandleClipRequest(AudioClipRequest request)
        {
            if (_mixerGroupToAudioSource.TryGetValue(request.AudioAsset.AudioMixerGroup, out var res))
                PlayRequestWith(request, res);
            else
                Debug.LogError($"Didn't find the AudioSource for {request.AudioAsset.AudioMixerGroup} mixer group");
        }
    }
}
