using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UISpriteAnimator : MonoBehaviour
{
    [SerializeField] Image _graphic;
    [SerializeField] Dictionary<string, SpriteSequence> _sequences;
    [SerializeField] bool _useUnscaledTime = true;
    [SerializeField] bool _playOnAwake;

    public bool IsPlaying => _animator != null;

    GraphicAnimator _animator;

    private void OnEnable()
    {
        if (_playOnAwake) Play(_sequences.First().Key);
    }

    private void OnDisable()
    {
        Stop();
    }

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
