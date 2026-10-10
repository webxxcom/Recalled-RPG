using Recalled.Core;
using UnityEngine;

namespace Recalled.Gameplay
{
    public class RequestClipDispatcher : MonoBehaviour
    {
        [Header("Raises")]
        [SerializeField] AudioClipRequestGameEvent _audioClipRequestChannel;

        public void Dispatch(SoundCue audioAsset)
        {
            _audioClipRequestChannel.Invoke(new(audioAsset, transform.position));
        }
    }
}
