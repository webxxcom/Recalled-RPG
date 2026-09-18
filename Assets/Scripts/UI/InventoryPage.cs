using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] InputActionReference _quickSlotRemove;
    [SerializeField] InputActionReference _quickSlotAdd;
    [SerializeField] Highlighter _highlighter;
    [SerializeField] QuickSlotsSO _quickSlots;
    [SerializeField] BagSO _generalItems;
    [SerializeField] InventorySO _inventory;

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

        var slot = EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>();
        var vec2 = context.ReadValue<Vector2>();

        if (_quickSlots.Set(vec2, slot.Item, out var replaced))
            _generalItems.Remove(slot.Item);
        if (!replaced.IsEmpty)
            _generalItems.Add(replaced);
    }

    void OnQuickSlotRemove(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            return;

        var slot = EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>();

        if (_generalItems.Add(slot.Item))
            _quickSlots.UnSet(slot.Item);
    }
}
