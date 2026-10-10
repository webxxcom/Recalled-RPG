using UnityEngine;

public class DamageDeath : HealthReactor
{
    [SerializeField] Animator _animator;
    [SerializeField] float _timeout = 1;

    protected override void OnDied(DamageInfo di)
    {
        _animator.SetTrigger(AnimatorParameters.DieHash);
        Destroy(transform.parent.gameObject, _timeout);
    }
}
