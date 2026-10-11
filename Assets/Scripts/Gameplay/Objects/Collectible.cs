using Recalled.Systems.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Collectible : MonoBehaviour
    {
        [SerializeField] ItemDefinition _definition;
        [SerializeField] int _count = 1;
        [SerializeField] UnityEvent _collected;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!collision.TryGetComponent<InventoryHolder>(out var holder))
                    return;

                _count = holder.Inventory.Add(_definition, _count);
                _collected?.Invoke();

                if (_count != 0)
                    return;

                Destroy(gameObject);
            }
        }
    }
}
