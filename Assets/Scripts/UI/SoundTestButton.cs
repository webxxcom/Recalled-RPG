using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class SoundTestButton : MonoBehaviour
{
    [SerializeField] AudioClip _playAudio;

    Button _button;
    AudioSource _audio;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
        => _button.onClick.AddListener(OnClick);
    private void OnDisable()
        => _button.onClick.RemoveListener(OnClick);

    void OnClick()
    {
        if (_audio.isPlaying) _audio.Stop();
        else _audio.PlayOneShot(_playAudio);
    }
}
