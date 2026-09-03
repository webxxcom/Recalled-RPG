using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class Collectible : MonoBehaviour
{
    [SerializeField] ItemDefinition _inventoryItemDefinition;
    [SerializeField] int _quantity;
    [SerializeField] AudioClip _pickUpSound;
    [SerializeField] InventorySO _inventory;

    bool _isCollected;
    Animator animator;
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        TryGetComponent(out animator);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCollected)
            return;

        if (collision.CompareTag("Player"))
        {
            if (!_inventory.Add(_inventoryItemDefinition, _quantity))
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
