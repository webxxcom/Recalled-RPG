using Newtonsoft.Json.Linq;
using Recalled.Gameplay;
using Recalled.Gameplay.Inventory;
using Recalled.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    public class QuickSlotEquipper : MonoBehaviour
    {
        [SerializeField] QuickSlotsAnchor _quickSlotsAnchor;
        [SerializeField] InputActionReference _quickSlotsAction;

        IQuickSlotsView _view;

        private void OnEnable()
        {
            _quickSlotsAction.action.performed += OnActionTriggered;
            _quickSlotsAnchor.Provided += OnAnchorProvided;

            if (_quickSlotsAnchor.IsSet) OnAnchorProvided(_quickSlotsAnchor.Value);
        }
        private void OnDisable()
        {
            _quickSlotsAction.action.performed -= OnActionTriggered;
            _quickSlotsAnchor.Provided -= OnAnchorProvided;
        }
        void OnAnchorProvided(IQuickSlotsView view) => _view = view;

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

            if (!eventSystem.currentSelectedGameObject.TryGetComponent<InventorySlot>(out var ivSlot))
                return;

            _view.RequestDefinitionForSlot(ivSlot.ItemInstance.Definition, i);
        }
    }
}
