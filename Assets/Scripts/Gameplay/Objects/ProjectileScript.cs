using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProjectileScript : MonoBehaviour
{
    [SerializeField] float _advancingSpeed;
    [SerializeField] int _dealtDamage;
    [SerializeField] float _knockbackPower;
    [SerializeField] float _timeToLive;
    [SerializeField] Vector2 _offset;

    Vector3 _direction;
    GameObject _owner;
    Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Initialize(GameObject owner, Vector2 destination, bool flipX)
    {
        _owner = owner;
        Vector2 pos = transform.position + new Vector3(_offset.x * (flipX ? -1 : 1), _offset.y);

        _direction = (destination - pos).normalized;
        transform.SetPositionAndRotation(
            pos,
            Quaternion.FromToRotation(Vector3.right, _direction));
        Destroy(gameObject, _timeToLive);
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _direction * _advancingSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_owner.tag))
            return;

        if (collision.TryGetComponent<HealthResource>(out var hp))
            hp.ApplyDamage(new(_knockbackPower, _dealtDamage, _owner, hp.Hurtbox));
        Destroy(gameObject);
    }
}
