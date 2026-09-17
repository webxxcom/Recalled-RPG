using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] InputActionReference _quickSlotAction;
    [SerializeField] Highlighter _highlighter;
    [SerializeField] QuickSlotsBook _quickSlots;
    [SerializeField] InventoryContent _content;
    [SerializeField] InventorySO _inventory;

    private void OnEnable()
    {
        _quickSlotAction.action.performed += OnQuickSlotToggle;
    }
    private void OnDisable()
    {
        _quickSlotAction.action.performed -= OnQuickSlotToggle;
    }

    void QuickSlotToggle(InventoryCell inventoryCell)
    {
        InventoryCell selected;
        ItemInstance item;
        if (inventoryCell.Item.IsQuickSlot)
        {
            if (_content.IsFull) return;

            item = _quickSlots.RemoveItemFromCell(inventoryCell);
            selected = _content.AddItemToView(item);
        }
        else
        {
            if (_quickSlots.IsFull) return;

            item = _content.RemoveItemFromView(inventoryCell);
            selected = _quickSlots.Add(item);
        }

        item.IsQuickSlot = !item.IsQuickSlot;
        _inventory.VisualsChanged();

        if (selected != null)
            EventSystem.current.SetSelectedGameObject(selected.gameObject);
    }

    void OnQuickSlotToggle(InputAction.CallbackContext _)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            QuickSlotToggle(EventSystem.current.currentSelectedGameObject.GetComponent<InventoryCell>());
    }
}
