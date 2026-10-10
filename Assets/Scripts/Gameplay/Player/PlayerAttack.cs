using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class PlayerAttack : EntityAttack
{
    [SerializeField] MeleeAttackSO _meleeAttackData;

    void OnAttack(InputValue value)
    {
        if (!value.isPressed || _timeSinceLastAttack < _meleeAttackData.ReloadTime)
            return;

        _timeSinceLastAttack = 0;
        Attack(_attackStrategies[0], new());
    }

    private void Update()
    {
        _timeSinceLastAttack += Time.deltaTime;
    }
}
