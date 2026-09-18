using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] InputActionReference _quickSlotAction;
    [SerializeField] Highlighter _highlighter;
    [SerializeField] LoadoutView _quickSlotsView;
    [SerializeField] LoadoutView _generalItemsView;
    [SerializeField] InventorySO _inventory;

    private void OnEnable()
    {
        _quickSlotAction.action.performed += OnQuickSlotToggle;
    }
    private void OnDisable()
    {
        _quickSlotAction.action.performed -= OnQuickSlotToggle;
    }

    void QuickSlotToggle(InventorySlot slot)
    {
        bool isInQuick = _quickSlotsView.Slots.Contains(slot);
        bool isInGeneral = _generalItemsView.Slots.Contains(slot);

        if (!isInQuick && !isInGeneral) return;

        LoadoutView from = isInGeneral ? _generalItemsView : _quickSlotsView;
        LoadoutView to = isInGeneral ? _quickSlotsView : _generalItemsView;

        if (to.Loadout.Add(slot.Item))
            from.Loadout.Remove(slot.Item);
    }

    void OnQuickSlotToggle(InputAction.CallbackContext _)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            QuickSlotToggle(EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>());
    }
}
