using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using Recalled.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    sealed class DiscardItem : MonoBehaviour
    {
        [SerializeField] InputActionReference _actionReference;
        [SerializeField] InventoryAnchor _inventoryAnchor;

        private void OnEnable() => _actionReference.action.performed += OnDiscarded;
        void OnDisable() => _actionReference.action.performed -= OnDiscarded;

        void OnDiscarded(InputAction.CallbackContext _)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            var current = eventSystem.currentSelectedGameObject;
            if (current == null || !current.TryGetComponent<InventorySlot>(out var slot))
                return;

            if (_inventoryAnchor.IsSet)
                _inventoryAnchor.Value.RequestDiscardItem(slot.ItemInstance);
            else Debug.LogWarning($"The {nameof(_inventoryAnchor)}'s value was not provided");
        }
    }
}
