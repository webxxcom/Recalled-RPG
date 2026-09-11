using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MovementBase
{
    [SerializeField] SprintingResource _playerSprinting;
    [SerializeField] Dash _playerDash;
    [SerializeField] PlayerCombatData _playerCombat;

    public bool IsSprinting => _playerSprinting.IsActive;

    void OnMove(InputValue value)
        => MovementIntention = value.Get<Vector2>();
    void OnDash(InputValue _)
        => _playerDash.TryDash(FacingDirection);
    void OnSprint(InputValue value)
        => _playerSprinting.Toggle(value.isPressed);

    protected override Vector2 GetMovementIntention()
    {
        if (!IsWalking)
            return Vector2.zero;

        Vector2 finalMovement = MovementIntention;
        return finalMovement / _playerCombat.Weight;
    }
}
