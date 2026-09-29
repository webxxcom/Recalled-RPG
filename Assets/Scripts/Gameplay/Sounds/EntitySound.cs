using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class EntitySound : MonoBehaviour
{
    protected AudioSource _audioSource;

    void Awake() => _audioSource = GetComponent<AudioSource>();
}
