using Recalled.Systems.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(InventoryHolder))]
    sealed class QuickSlotsHolder : MonoBehaviour
    {
        [SerializeField] InputActionReference _useQuickSlotAction;
        [SerializeField] List<ItemDefinition> _loadout;

        [Header("Sets")]
        [SerializeField] QuickSlotsAnchor _slotsAnchor;

        QuickSlots _quickSlots;

        private void Awake() => _quickSlots = new(GetComponent<InventoryHolder>().Inventory);
        private void Start()
        {
            int i = 0;
            foreach (var item in _loadout)
                _quickSlots.Put(i++, item);

            _slotsAnchor.Provide(_quickSlots);
        }
        private void OnEnable() => _useQuickSlotAction.action.performed += OnQuickSlotAction;
        private void OnDisable() => _useQuickSlotAction.action.performed -= OnQuickSlotAction;
        private void OnDestroy() => _slotsAnchor.Withdraw(_quickSlots);

        void OnQuickSlotAction(InputAction.CallbackContext context)
        {
            var vec2 = context.ReadValue<Vector2>();

            int i = -1;
            if (vec2 == Vector2.left) i = 0;
            else if (vec2 == Vector2.up) i = 1;
            else if (vec2 == Vector2.right) i = 2;
            else if (vec2 == Vector2.down) i = 3;

            if (i == -1)
                return;

            _quickSlots.Use(i, gameObject);
        }
    }
}
