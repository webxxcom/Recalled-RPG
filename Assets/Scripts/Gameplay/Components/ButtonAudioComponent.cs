using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonAudioComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] AudioClip _onHover;
    [SerializeField] AudioClip _onClick;

    AudioSource _audioSource;

    IEnumerator PlayClickSound()
    {
        AudioSource audioSource = Instantiate(_audioSource);

        audioSource.PlayOneShot(_onClick);

        yield return new WaitWhile(() => audioSource.isPlaying);

        Destroy(audioSource.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(PlayClickSound());
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioSource.PlayOneShot(_onHover);
    }
}
