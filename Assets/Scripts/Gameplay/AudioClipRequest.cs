using Recalled.Core;
using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    public readonly struct AudioClipRequest
    {
        public readonly SoundCue AudioAsset;
        public readonly Vector2 Position;

        public AudioClipRequest(SoundCue audioAsset, Vector2 position)
        {
            if (audioAsset)
                throw new ArgumentNullException();

            AudioAsset = audioAsset;
            Position = position;
        }
    }
}
