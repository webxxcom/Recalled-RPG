using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{

    [RequireComponent(typeof(Sprint))]
    [RequireComponent(typeof(Dash))]
    public sealed class PlayerMovement : Movementable
    {
        Sprint _sprintingResource;
        Dash _dash;

        public bool IsSprinting => _sprintingResource.IsActive;

        private void Awake()
        {
            _sprintingResource = GetComponent<Sprint>();
            _dash = GetComponent<Dash>();
        }

        void OnMove(InputValue value)
            => MovementIntention = value.Get<Vector2>();
        void OnDash(InputValue _)
            => _dash.TryDash(FacingDirection);
        void OnSprint(InputValue value)
            => _sprintingResource.IsActive = value.isPressed;
    }
}
