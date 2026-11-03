using Recalled.UI;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
internal class SpeakerDialogueController : MonoBehaviour
{
    [SerializeField] Image _facesetImage;
    [SerializeField] Image _emotionImage;
    [SerializeField] TypeWriter _typeWriter;

    AudioSource _audioSource;
    public bool IsDone => !_typeWriter.IsTyping;

    void Awake() => _audioSource = GetComponent<AudioSource>();

    public void Init(Sprite faceset)
    {
        _facesetImage.sprite = faceset;
    }

    private void OnDisable()
    {
        _emotionImage.enabled = true;
    }

    public void Speak(string text, Sprite emotionSprite)
    {
        _audioSource.Play();
        _emotionImage.sprite = emotionSprite;
        _emotionImage.preserveAspect = true;
        _emotionImage.enabled = true;
        _typeWriter.Play(text);
    }

    public void Finish()
    {
        _audioSource.Stop();
        _typeWriter.Stop();
    }
}