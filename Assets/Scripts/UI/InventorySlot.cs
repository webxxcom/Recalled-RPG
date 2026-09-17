using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public sealed class InventorySlot : InventoryCell
{
    [SerializeField] Selectable _selectable;

    public override void SetItem(ItemInstance item)
    {
        base.SetItem(item);

        _selectable.enabled = true;
    }

    public override ItemInstance RemoveItem()
    {
        _selectable.enabled = false;

        return base.RemoveItem();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_selectable == null) _selectable = GetComponent<Selectable>();
    }
#endif
}
