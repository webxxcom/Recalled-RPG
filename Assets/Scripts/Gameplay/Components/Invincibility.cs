using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class Invincibility : MonoBehaviour
{
    [SerializeField] float _duration;
    [SerializeField] HealthResource _healthProvider;

    private void OnEnable() => _healthProvider.HpChangeApplied += OnValueChanged;
    private void OnDisable() => _healthProvider.HpChangeApplied -= OnValueChanged;
    private void OnValueChanged(DamageInfo _) => BecomeInvinsibleFor(_duration);

    public void BecomeInvinsibleFor(float seconds)
        => _healthProvider.GrantInvincibility(seconds);
}
