using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SpeakerDialogueController : MonoBehaviour
{
    [SerializeField] Image _facesetImage;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _delayTime = 0.035f;

    public bool IsSpeaking => _text.maxVisibleCharacters < _text.text.Length;

    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(DialogueContext context)
    {
        _facesetImage.sprite = context.faceset;
    }

    public void FinishText(string text)
    {
        _text.text = text;
        _text.maxVisibleCharacters = _text.text.Length;
        _audioSource.Stop();

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }

    Coroutine _currentCoroutine;
    public void StartRevealDialogueText(string text)
    {
        _audioSource.Play();

        _currentCoroutine = StartCoroutine(Utils.RevealTextOverTime(_text, _delayTime, text));
    }
}