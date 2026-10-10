using UnityEngine;

[RequireComponent(typeof(HealthResource))]
public abstract class HealthReactor : MonoBehaviour
{
    [SerializeField] protected HealthResource _health;

    private void OnEnable()
    {
        _health.Died += OnDied;
        _health.HpChanged += OnHpChanged;
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;
        _health.HpChanged -= OnHpChanged;
    }

    protected virtual void OnHpChanged(DamageInfo di) { }
    protected virtual void OnDied(DamageInfo di) { }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_health == null)
            _health = GetComponent<HealthResource>();
    }
#endif
}
