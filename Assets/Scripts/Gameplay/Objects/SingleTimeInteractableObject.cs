using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public abstract class SingleTimeInteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] AudioClip _interactAudio;
    [SerializeField] AudioMixerGroup _audioGroup;

    bool _IsInteracted;
    public bool IsInteracted
    {
        get => _IsInteracted;
        protected set
        {
            if (!value) // Can only be set to `true'
                return;

            _IsInteracted = value;
            _audioSource.PlayOneShot(_interactAudio);
            OnInteract?.Invoke();
        }
    }

    AudioSource _audioSource;

    public event Action OnInteract;

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.outputAudioMixerGroup = _audioGroup;
    }

    public abstract bool PlayerCanInteract();
    public abstract void Interact();
}
