using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UISpriteAnimator : MonoBehaviour
{
    [SerializeField] Image _graphic;
    [SerializeField] Dictionary<string, SpriteSequence> _sequences;
    [SerializeField] bool _useUnscaledTime = true;

    /// <summary>Raised when a non-looping sequence reaches its end.</summary>
    public event Action Completed;

    private float _time;
    private int _index;

    SpriteSequence _currentPlaying;
    public bool IsPlaying => _currentPlaying != null;

    private void OnDisable()
    {
        _currentPlaying = null;
    }

    public void Play(string clip)
    {
        if (_sequences.TryGetValue(clip, out var sequence))
            Play(sequence);
        else
            Debug.LogError($"{name}: couldn't find a sequence with name {clip}", this);
    }

    void Play(SpriteSequence sequence)
    {
        if (sequence == null || !sequence.IsValid)
        {
            Debug.LogWarning($"{name}: no valid sprite sequence to play", this);
            Stop();
            return;
        }

        _time = 0f;
        _index = 0;
        _currentPlaying = sequence;

        ApplyFrameAt(0f);
    }

    public void Stop()
    {
        _currentPlaying = null;
    }

    public void SkipToEnd()
    {
        if (_currentPlaying == null || !_currentPlaying.IsValid)
            return;

        _time = _currentPlaying.Length;
        ApplyFrameAt(_time);
        Stop();
    }

    private void Update()
    {
        if (!IsPlaying)
            return;

        _time += _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

        if (_time >= _currentPlaying.Length)
        {
            if (_currentPlaying.Loop)
            {
                _time %= _currentPlaying.Length;
                _index = 0;
            }
            else
            {
                _time = _currentPlaying.Length;
                ApplyFrameAt(_time);
                Stop();

                Completed?.Invoke();
                return;
            }
        }

        ApplyFrameAt(_time);
    }

    void ApplyFrameAt(float time)
    {
        var frames = _currentPlaying.Frames;

        while (_index + 1 < frames.Count && frames[_index + 1].Time <= time)
            _index++;

        Sprite sprite = frames[_index].Sprite;

        // Write only on change to avoid dirtying the canvas
        if (_graphic.sprite != sprite)
            _graphic.sprite = sprite;
    }
}
