using Recalled.Systems.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay.Inventory
{
    [RequireComponent(typeof(InventoryHolder))]
    [RequireComponent(typeof(ItemProcessor))]
    internal sealed class QuickSlotsHolder : MonoBehaviour
    {
        [SerializeField] ItemDefinition[] _loadout;

        [Header("Sets")]
        [SerializeField] QuickSlotsAnchor _slotsAnchor;

        public QuickSlots Reference { get; private set; }
        ItemProcessor _itemUser;


        private void Awake() => _itemUser = GetComponent<ItemProcessor>();
        private void Start()
        {
            Reference = new(_loadout, GetComponent<InventoryHolder>().Reference);

            _slotsAnchor.Provide(Reference);
        }
        private void OnDestroy() => _slotsAnchor.Withdraw(Reference);

        void OnQuickSlots(InputValue value)
        {
            var vec2 = value.Get<Vector2>();

            int i = -1;
            if (vec2 == Vector2.left) i = 0;
            else if (vec2 == Vector2.up) i = 1;
            else if (vec2 == Vector2.right) i = 2;
            else if (vec2 == Vector2.down) i = 3;

            if (i == -1)
                return;

            if (Reference[i]) _itemUser.Consume(Reference[i]);
        }
    }
}
