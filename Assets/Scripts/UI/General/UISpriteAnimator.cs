using Recalled.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI
{
    [DisallowMultipleComponent]
    public sealed class UISpriteAnimator : MonoBehaviour
    {
        class GraphicAnimator
        {
            public bool IsCompleted { get; private set; }

            readonly SpriteSequence _sequence;
            public GraphicAnimator(SpriteSequence sequence)
            {
                _sequence = sequence;
            }

            float _time;
            int _index;

            /// <summary>Update animation state frame and returns current sprite</summary>
            public Sprite Update(bool useUnscaledTime)
            {
                _time += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                if (_time >= _sequence.Length)
                {
                    if (_sequence.Loop)
                    {
                        _time %= _sequence.Length;
                        _index = 0;
                    }
                    else
                    {
                        _time = _sequence.Length;
                        IsCompleted = true;
                        return ApplyFrameAt(_time);
                    }
                }

                return ApplyFrameAt(_time);
            }

            Sprite ApplyFrameAt(float time)
            {
                var frames = _sequence.Frames;
                while (_index + 1 < frames.Count && frames[_index + 1].Time <= time)
                    _index++;

                return frames[_index].Sprite;
            }
        }

        [SerializeField] Image _graphic;
        [SerializeField] Dictionary<string, SpriteSequence> _sequences;
        [SerializeField] bool _useUnscaledTime = true;

        public bool IsPlaying => _animator != null;

        GraphicAnimator _animator;

        private void OnDisable() => Stop();

        public void Play(string clip)
        {
            if (_sequences.TryGetValue(clip, out var sequence))
                _animator = new(sequence);
            else
                Debug.LogError($"{name}: couldn't find a sequence with name {clip}", this);
        }

        public void Stop()
        {
            _animator = null;
        }

        private void Update()
        {
            if (!IsPlaying || !_graphic.enabled)
                return;

            Sprite sprite = _animator.Update(_useUnscaledTime);
            if (sprite != _graphic.sprite)
                _graphic.sprite = sprite;
        }
    }
}
