using Recalled.Gameplay.Movement;
using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class MeleeAttackCollisionMove : MonoBehaviour
    {
        [SerializeField] Movementable _movementBase;

        Collider2D _hitbox;

        private void Awake()
        {
            _hitbox = GetComponent<Collider2D>();
        }

        private void OnEnable()
            => _movementBase.OnMovement += SetAttackCollisionOffset;
        private void OnDisable()
            => _movementBase.OnMovement -= SetAttackCollisionOffset;

        void SetAttackCollisionOffset()
        {
            _hitbox.transform.rotation
                = Quaternion.FromToRotation(Vector2.right, _movementBase.MovementIntention);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_movementBase == null)
                _movementBase = GetComponentInParent<Movementable>();
        }
#endif
    }
}
