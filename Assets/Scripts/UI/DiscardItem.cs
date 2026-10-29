using Recalled.Core;
using Recalled.Gameplay.Inventory;
using Recalled.Systems.Inventory;
using Recalled.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Recalled.UI
{
    public class DiscardItem : MonoBehaviour
    {
        [SerializeField] InputActionReference _actionReference;
        [SerializeField] InventoryAnchor _inventoryAnchor;

        IBagView _inventoryView;

        private void OnEnable()
        {
            _inventoryAnchor.Provided += OnInventoryProvided;
            _actionReference.action.performed += OnDiscarded;

            if (_inventoryAnchor.IsSet) OnInventoryProvided(_inventoryAnchor.Value);
        }
        void OnDisable()
        {
            _inventoryAnchor.Provided -= OnInventoryProvided;
            _actionReference.action.performed -= OnDiscarded;
        }

        void OnInventoryProvided(IBagView inventoryView) => _inventoryView = inventoryView;
        void OnDiscarded(InputAction.CallbackContext _)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            var current = eventSystem.currentSelectedGameObject;
            if (current == null || !current.TryGetComponent<InventorySlot>(out var slot))
                return;

            _inventoryView.RequestDiscardItem(slot.ItemInstance);
        }
    }
}
