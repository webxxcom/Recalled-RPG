using UnityEngine;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]

[RequireComponent(typeof(Lootable))]
public class Collectible : MonoBehaviour
{
    [SerializeField] AudioClip _pickUpSound;
    [SerializeField] InventorySO _inventory;

    bool _isCollected;
    Animator animator;
    AudioSource _audioSource;
    Lootable _lootable;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _lootable = GetComponent<Lootable>();

        TryGetComponent(out animator);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCollected)
            return;

        if (collision.CompareTag("Player"))
        {
            if (!_lootable.LootItem())
                return;

            _isCollected = true;
            if (animator) animator.SetTrigger(AnimatorParameters.CollectedHash);
            _audioSource.PlayOneShot(_pickUpSound);
        }
    }
}
