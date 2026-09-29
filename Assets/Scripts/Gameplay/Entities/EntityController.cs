using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EntityController : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Collider2D Collider2D { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRendererGroup SpriteRendererGroup { get; private set; }

    protected virtual void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();

        Animator = GetComponentInChildren<Animator>();
        SpriteRendererGroup = GetComponentInChildren<SpriteRendererGroup>();
    }
}
