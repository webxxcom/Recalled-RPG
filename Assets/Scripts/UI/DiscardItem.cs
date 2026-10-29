using Recalled.Gameplay;
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
        [SerializeField] ItemProcessorAnchor _itemProcessorAnchor;

        void OnEnable() => _actionReference.action.performed += OnDiscarded;
        void OnDisable() => _actionReference.action.performed -= OnDiscarded;

        void OnDiscarded(InputAction.CallbackContext _)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            var current = eventSystem.currentSelectedGameObject;
            if (current == null || !current.TryGetComponent<InventorySlot>(out var slot))
                return;

            if (_itemProcessorAnchor.IsSet)
                _itemProcessorAnchor.Value.TryDiscard(slot.ItemInstance);
            else Debug.LogWarning($"The {nameof(_itemProcessorAnchor)}'s value was not provided");
        }
    }
}
