using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public sealed class InventorySlot : InventoryCell
{
    [SerializeField] Selectable _selectable;

    public override bool SetItem(ItemInstance item)
    {
        if (base.SetItem(item))
        {
            _selectable.enabled = true;
            return true;
        }
        return false;
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
