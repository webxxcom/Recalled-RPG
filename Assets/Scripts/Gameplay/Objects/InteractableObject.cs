using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] AudioClip _firstStateAudio;
    [SerializeField] AudioClip _secondStateAudio;
    [SerializeField] AudioMixerGroup _audioGroup;

    public bool IsInteracted
    {
        get => _IsInteracted;
        protected set
        {
            if (value)
            {
                _audioSource.PlayOneShot(_firstStateAudio);
                OnInteract?.Invoke();
            }
            else
            {
                if (_secondStateAudio) _audioSource.PlayOneShot(_secondStateAudio);
            }
            _IsInteracted = value;
        }
    }

    bool _IsInteracted;
    AudioSource _audioSource;

    public event Action OnInteract;

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioGroup == null)
            throw new MissingReferenceException($"Missing audio group on {GetType().Name}");
    }

    protected virtual void Start()
    {
        _audioSource.outputAudioMixerGroup = _audioGroup;
    }


    // Method used in the trigger to decide if at the current moment player can interact with the object
    // whether it's an availability of a key in player's inventory to open a chest or a specific looking into the picture
    public abstract bool PlayerCanInteract();

    public abstract void Interact();
}
