using UnityEngine;

public class AttackSound : EntitySound
{
    [SerializeField] AudioClip _attackSound;
    [SerializeField] EntityAttack entityAttackComponent;

    private void OnEnable()
    {
        entityAttackComponent.OnAttackStarted += HandleAttackSound;
    }

    private void OnDisable()
    {
        entityAttackComponent.OnAttackStarted -= HandleAttackSound;
    }

    void HandleAttackSound()
    {
         _audioSource.PlayOneShot(_attackSound);
    }
}
