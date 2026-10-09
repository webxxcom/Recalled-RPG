using Recalled.Systems.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] Image _icon;

    public ItemSlotsArray.Slot Slot { get; private set; }

    public virtual void SetSlot(ItemSlotsArray.Slot slot)
    {
        if (slot == null) return;

        Slot = slot;
        _icon.sprite = Slot.Item.Definition.Icon;
        _icon.preserveAspect = true;
        _icon.enabled = true;
    }

    public virtual void RemoveSlot()
    {
        Slot = null;
        _icon.enabled = false;
    }
}
