using UnityEngine;

public class EntityMovementSound : EntitySound
{
    [SerializeField] AudioClip _walkingSound;
    [SerializeField] float _delayBetween;
    [SerializeField] MovementBase _movementBase;

    bool _isPlaying;

    void OnEnable()
    {
        _movementBase.OnMovementStarted += StartPlaying;
        _movementBase.OnMovementStopped += StopPlaying;
    }

    void OnDisable()
    {
        _movementBase.OnMovementStarted -= StartPlaying;
        _movementBase.OnMovementStopped -= StopPlaying;
    }

    void StartPlaying()
    {
        _isPlaying = true;
        _elapsed = float.MaxValue;
    }

    void StopPlaying() => _isPlaying = false;

    float _elapsed = 0;
    void UpdateMovementSound()
    {
        _elapsed += Time.deltaTime;

        if (_movementBase.IsWalking && _elapsed > (_delayBetween / _movementBase.CurrentSpeed))
        {
            _audioSource.PlayOneShot(_walkingSound);

            _elapsed = 0;
        }
    }

    private void Update()
    {
        if (_isPlaying)
            UpdateMovementSound();
    }
}
