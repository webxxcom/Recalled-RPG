using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{

    [RequireComponent(typeof(SprintingResource))]
    [RequireComponent(typeof(Dash))]
    public sealed class PlayerMovement : Movementable
    {
        SprintingResource _sprintingResource;
        Dash _dash;

        public bool IsSprinting => _sprintingResource.IsActive;

        private void Awake()
        {
            _sprintingResource = GetComponent<SprintingResource>();
            _dash = GetComponent<Dash>();
        }

        void OnMove(InputValue value)
            => MovementIntention = value.Get<Vector2>();
        void OnDash(InputValue _)
            => _dash.TryDash(FacingDirection);
        void OnSprint(InputValue value)
            => _sprintingResource.Toggle(value.isPressed);
    }
}
