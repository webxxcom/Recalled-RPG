using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class PlayerAttack : EntityAttack
    {
        [SerializeField] MeleeAttackSO _meleeAttackData;

        void OnAttack(InputValue value)
        {
            if (!value.isPressed || _elapsedLastAttack < _meleeAttackData.ReloadTime)
                return;

            _elapsedLastAttack = 0;
            Attack(_attackStrategies[0], new());
        }

        private void Update()
        {
            _elapsedLastAttack += Time.deltaTime;
        }
    }
}
