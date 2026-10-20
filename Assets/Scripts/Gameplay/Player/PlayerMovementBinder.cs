using Recalled.Gameplay.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Sprint))]
    [RequireComponent(typeof(Dash))]
    [RequireComponent(typeof(Movementable))]
    public sealed class PlayerMovementBinder : MonoBehaviour
    {
        Sprint _sprintingResource;
        Dash _dash;
        Movementable _movementable;

        void Awake()
        {
            _sprintingResource = GetComponent<Sprint>();
            _dash = GetComponent<Dash>();
            _movementable = GetComponent<Movementable>();
        }

        void OnMove(InputValue value)
            => _movementable.SetDirection(value.Get<Vector2>());
        void OnDash(InputValue _)
            => _dash.TryDash(_movementable.FacingDirection);
        void OnSprint(InputValue value)
            => _sprintingResource.IsActive = value.isPressed;
    }
}
