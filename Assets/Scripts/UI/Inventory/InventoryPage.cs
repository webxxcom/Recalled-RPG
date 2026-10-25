using Recalled.Gameplay;
using Recalled.Systems.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Recalled.UI.Inventory
{
    sealed class InventoryPage : MonoBehaviour
    {
        [SerializeField] InputActionReference _quickSlotRemove;
        [SerializeField] InputActionReference _quickSlotAdd;
        [SerializeField] InventoryVariable _inventory;

        private void OnEnable()
        {
            _quickSlotRemove.action.performed += OnQuickSlotRemove;
            _quickSlotAdd.action.performed += OnQuickSlotItemPressed;
        }
        private void OnDisable()
        {
            _quickSlotRemove.action.performed -= OnQuickSlotRemove;
            _quickSlotAdd.action.performed -= OnQuickSlotItemPressed;
        }

        void OnQuickSlotItemPressed(InputAction.CallbackContext context)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                return;

        }

        void OnQuickSlotRemove(InputAction.CallbackContext context)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                return;

            var slot = EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>();

            //if (_generalItems.Add(slot.Item))
            //{
            //    _quickSlots.UnSet(slot.Item);

            //    SetCurrentSelected();
            //}
        }
    }
}
