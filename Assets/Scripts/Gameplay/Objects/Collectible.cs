using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Collectible : MonoBehaviour
    {
        [SerializeField] ItemDefinition _definition;
        [SerializeField] int _count = 1;

        IReactor[] _reactors;

        private void Awake()
        {
            _reactors = GetComponentsInChildren<IReactor>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!TryGetComponent<InventoryHolder>(out var holder))
                    return;

                if (holder.Inventory.Add(_definition, _count) != 0)
                    return;

                foreach (var reactor in _reactors)
                    reactor.React();

                // First let reactors react and then destroy
                Destroy(gameObject);
            }
        }
    }
}
