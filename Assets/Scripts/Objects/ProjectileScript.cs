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
    EntityController _owner;
    Rigidbody2D _rigidbody2D;

    public void Initialize(EntityController owner, Vector3 destination, bool flipX)
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _owner = owner;

        Vector3 pos = (Vector2)transform.position + new Vector2(_offset.x * (flipX ? -1 : 1), _offset.y);

        _direction = (destination - pos).normalized;
        transform.SetPositionAndRotation(
            pos,
            Quaternion.FromToRotation(Vector3.right, _direction));
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _direction * _advancingSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_owner.tag))
            return;

        if (collision.TryGetComponent(out HealthResource hp))
            hp.ApplyDamage(new(_dealtDamage, _knockbackPower, _owner, hp.Hurtbox));
        Destroy(gameObject);
    }

    float _elapsedLivingTime;
    private void Update()
    {
        if (_elapsedLivingTime < _timeToLive)
            _elapsedLivingTime += Time.deltaTime;
        else
            Destroy(gameObject);
    }
}
