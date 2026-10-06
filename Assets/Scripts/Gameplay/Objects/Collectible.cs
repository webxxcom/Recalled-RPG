using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Collectible : MonoBehaviour
    {
        [SerializeField] ItemInstance _looted;
        [SerializeField] InventorySO _inventory;

        IReactor[] _reactors;

        private void Awake()
        {
            _reactors = GetComponentsInChildren<IReactor>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!_inventory.GeneralItems.Add(_looted))
                    return;

                foreach (var reactor in _reactors)
                    reactor.React();

                // First let reactors react and then destroy
                Destroy(gameObject);
            }
        }
    }
}
