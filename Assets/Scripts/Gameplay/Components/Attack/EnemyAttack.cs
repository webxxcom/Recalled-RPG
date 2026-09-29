using UnityEngine;

public class EnemyAttack : EntityAttack
{
    [SerializeField] TargetProviderSO _targetProviderSO;
    [SerializeField] float _intervalBetweenStates;

    TargetProvider _targetProviderInstance;

    private void Awake()
    {
        _targetProviderInstance = _targetProviderSO.CreateInstance();
    }

    void Update()
    {
        if (_timeSinceLastAttack > _intervalBetweenStates && _targetProviderInstance.HasTarget && gameObject.activeInHierarchy)
        {
            foreach (var attackStrategy in _attackStrategies)
            {
                AttackContext attackContext = new(_targetProviderInstance.CurrentTarget);
                if (attackStrategy.CanAttack(attackContext))
                {
                    Attack(attackStrategy, attackContext);
                    _timeSinceLastAttack = 0;
                    break;
                }
            }
        }
        _timeSinceLastAttack += Time.deltaTime;
    }
}
