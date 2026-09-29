using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeAttackCollisionMove : MonoBehaviour
{
    [SerializeField] MovementBase _movementBase;
    [SerializeField] Collider2D _hitbox;

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
            _movementBase = GetComponentInParent<MovementBase>();
        if (_hitbox == null)
            _hitbox = GetComponent<Collider2D>();
    }
#endif
}
