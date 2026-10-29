using Recalled.Gameplay.Inventory;
using Recalled.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    sealed class QuickSlotEquipper : MonoBehaviour
    {
        [SerializeField] QuickSlotsAnchor _quickSlotsAnchor;
        [SerializeField] InputActionReference _quickSlotsAction;

        private void OnEnable() => _quickSlotsAction.action.performed += OnActionTriggered;
        private void OnDisable() => _quickSlotsAction.action.performed -= OnActionTriggered;

        void OnActionTriggered(InputAction.CallbackContext context)
        {
            var vec2 = context.ReadValue<Vector2>();

            int i = -1;
            if (vec2 == Vector2.left) i = 0;
            else if (vec2 == Vector2.up) i = 1;
            else if (vec2 == Vector2.right) i = 2;
            else if (vec2 == Vector2.down) i = 3;

            if (i == -1)
                return;

            PutIntoSlot(i);
        }

        void PutIntoSlot(int i)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            var current = eventSystem.currentSelectedGameObject;
            if (current == null || !current.TryGetComponent<InventorySlot>(out var ivSlot))
                return;

            if (_quickSlotsAnchor.IsSet)
                _quickSlotsAnchor.Value.RequestDefinitionForSlot(ivSlot.ItemInstance.Definition, i);
            else Debug.LogWarning($"The {nameof(_quickSlotsAnchor)} was not provided");
        }
    }
}
