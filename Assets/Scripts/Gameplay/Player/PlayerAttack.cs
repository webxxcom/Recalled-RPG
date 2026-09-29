using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class PlayerAttack : EntityAttack
{
    [SerializeField] PlayerCombatData _playerCombatData;

    void OnAttack(InputValue value)
    {
        //TODO player attack strategy
        if (value.isPressed && _timeSinceLastAttack >= _playerCombatData.ReloadTime)
        {
            _timeSinceLastAttack = 0;

            Attack(_attackStrategies[0], new(null));
        }
    }

    private void Update()
    {
        _timeSinceLastAttack += Time.deltaTime;
    }
}
