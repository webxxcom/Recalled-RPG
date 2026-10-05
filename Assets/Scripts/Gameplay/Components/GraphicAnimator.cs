using UnityEngine;

public class GraphicAnimator
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
