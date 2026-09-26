using UnityEngine;

[RequireComponent(typeof(HealthResource))]
public abstract class HealthReactor : MonoBehaviour
{
    [SerializeField] protected HealthResource _health;

    private void OnEnable()
    {
        _health.OnDeath += OnDeath;
        _health.OnHpChangeApplied += OnHpChangeApplied;
        _health.OnHpChange += OnHpChange;
    }

    private void OnDisable()
    {
        _health.OnDeath -= OnDeath;
        _health.OnHpChangeApplied -= OnHpChangeApplied;
        _health.OnHpChange -= OnHpChange;
    }

    protected virtual void OnHpChange(DamageInfo di) { }
    protected virtual void OnHpChangeApplied(DamageInfo di) { }
    protected virtual void OnDeath(DamageInfo di) { }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_health == null)
            _health = GetComponent<HealthResource>();
    }
#endif
}
