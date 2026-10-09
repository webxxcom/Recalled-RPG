using Recalled.Systems.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public sealed class InventorySlotView : InventoryCell
{
    [SerializeField] Selectable _selectable;
    [SerializeField] TextMeshProUGUI _countText;

    public override void SetSlot(ItemSlotsArray.Slot slot)
    {
        base.SetSlot(slot);

        _selectable.enabled = true;
        if (slot.Item.Definition.IsStackable)
        {
            _countText.enabled = true;
            _countText.text = slot.Item.Count.ToString();
        }
        else
            _countText.enabled = false;
    }

    public override void RemoveSlot()
    {
        _selectable.enabled = false;
        _countText.enabled = false;

        base.RemoveSlot();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_selectable == null) _selectable = GetComponent<Selectable>();
    }
#endif
}
