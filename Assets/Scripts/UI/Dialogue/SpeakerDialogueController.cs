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
    [SerializeField] TMP_Text _text;
    [SerializeField] float _delayTime = 0.035f;

    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(Sprite faceset)
    {
        _facesetImage.sprite = faceset;
    }

    public IEnumerator Speak(string text, InputAction skipAction)
    {
        _audioSource.Play();

        // Typewrite until skip action
        yield return TypeWriter.TypeWriteWithSkip(_text, _delayTime, text, skipAction);

        _audioSource.Stop();
    }
}