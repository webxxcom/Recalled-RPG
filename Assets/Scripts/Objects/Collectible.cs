using UnityEngine;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]

[RequireComponent(typeof(Lootable))]
public class Collectible : MonoBehaviour
{
    [SerializeField] ItemDefinition _inventoryItemDefinition;
    [SerializeField] int _quantity;
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
            if (!_inventory.AddItem(_inventoryItemDefinition.CreateInstance(_quantity)))
                return;

            _isCollected = true;
            if (animator) animator.SetTrigger(AnimatorParameters.CollectedHash);
            _audioSource.PlayOneShot(_pickUpSound);
        }
    }

    static public Collectible Instantiate(ItemDefinition inventoryItem, int quantity)
    {
        return new()
        {
            _inventoryItemDefinition = inventoryItem,
            _quantity = quantity
        };
    }
}
