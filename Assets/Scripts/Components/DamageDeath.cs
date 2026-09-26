using UnityEngine;

public class DamageDeath : HealthReactor
{
    [SerializeField] Animator _animator;
    [SerializeField] Behaviour[] _toDisable;

    protected override void OnDeath(DamageInfo di)
    {
        _animator.SetTrigger(AnimatorParameters.DieHash);
        Destroy(transform.parent.gameObject, 50);

        foreach (var item in _toDisable)
            item.enabled = false;
    }
}
