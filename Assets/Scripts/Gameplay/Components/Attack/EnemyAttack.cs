using Recalled.Gameplay;
using UnityEngine;

public class EnemyAttack : EntityAttack
{
    [SerializeField] ITargetProvider _targetProvider;
    [SerializeField] float _intervalBetweenStates;

    private void Awake()
    {
        _targetProvider = transform.parent.GetComponent<ITargetProvider>();
    }

    void Update()
    {
        if (_elapsedLastAttack < _intervalBetweenStates)
            return;

        if (_targetProvider.HasTarget)
        {
            foreach (var attackStrategy in _attackStrategies)
            {
                AttackContext attackContext = new(_targetProvider.Target);
                if (attackStrategy.CanAttack(attackContext))
                {
                    Attack(attackStrategy, attackContext);
                    _elapsedLastAttack = 0;
                    break;
                }
            }
        }
        _elapsedLastAttack += Time.deltaTime;
    }
}
