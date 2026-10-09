using Recalled.Gameplay;
using Recalled.Systems.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] InputActionReference _quickSlotRemove;
    [SerializeField] InputActionReference _quickSlotAdd;
    [SerializeField] InventoryVariable _inventory;
    [SerializeField] Highlighter _highlighter;

    private void OnEnable()
    {
        _quickSlotRemove.action.performed += OnQuickSlotRemove;
        _quickSlotAdd.action.performed += OnQuickSlotItemPressed;

        _highlighter.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        _quickSlotRemove.action.performed -= OnQuickSlotRemove;
        _quickSlotAdd.action.performed -= OnQuickSlotItemPressed;

        _highlighter.gameObject.SetActive(false);
    }

    void OnQuickSlotItemPressed(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            return;

        var slot = EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlotView>();
        if (slot.Slot.Item.Definition.Category != ItemCategory.Consumable)
            return;

        var vec2 = context.ReadValue<Vector2>();

        //if (_quickSlots.Set(vec2, slot.Item, out var replaced))
        //{
        //    _generalItems.Remove(slot.Item);

        //    SetCurrentSelected();
        //}
        //if (!replaced.IsEmpty)
        //    _generalItems.Add(replaced);
    }

    void OnQuickSlotRemove(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            return;

        var slot = EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlotView>();

        //if (_generalItems.Add(slot.Item))
        //{
        //    _quickSlots.UnSet(slot.Item);

        //    SetCurrentSelected();
        //}
    }
}
