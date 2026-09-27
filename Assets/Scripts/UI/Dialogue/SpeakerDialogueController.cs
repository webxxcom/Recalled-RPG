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

    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(DialogueContext context)
    {
        _facesetImage.sprite = context.faceset;
    }

    public IEnumerator RevealDialogueText(string text)
    {
        _audioSource.Play();

        yield return Utils.RevealTextOverTime(_text, _delayTime, text);

        _audioSource.Stop();
    }
}