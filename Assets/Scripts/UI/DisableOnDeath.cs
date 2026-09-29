using UnityEngine;

public class DisableOnDeath : MonoBehaviour
{
    [SerializeField] HealthResource _health;
    [SerializeField] Behaviour[] _toDisable;

    private void OnEnable()
    {
        _health.Died += DisableAll;
    }
    private void OnDisable()
    {
        _health.Died -= DisableAll;
    }

    void DisableAll(DamageInfo _)
    {
        foreach (var behaviour in _toDisable)
            behaviour.enabled = false;
    }
}
