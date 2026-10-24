using Recalled.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
internal class SpeakerDialogueController : MonoBehaviour
{
    [SerializeField] Image _facesetImage;
    [SerializeField] Image _emotionImage;
    [SerializeField] TypeWriter _typeWriter;

    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(Sprite faceset)
    {
        _facesetImage.sprite = faceset;
    }

    private void OnDisable()
    {
        _emotionImage.enabled = true;
    }

    public IEnumerator Speak(string text, Sprite emotionSprite)
    {
        _audioSource.Play();
        _emotionImage.sprite = emotionSprite;
        _emotionImage.preserveAspect = true;
        _emotionImage.enabled = true;

        // Typewrite until skip action
        yield return _typeWriter.TypeWriteWithSkip(text);

        _audioSource.Stop();
    }
}