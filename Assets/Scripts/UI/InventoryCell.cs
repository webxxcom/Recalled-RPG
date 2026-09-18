using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] Image _icon;

    public ItemInstance Item { get; private set; }
    public bool HasItem => Item != null;

    public virtual bool SetItem(ItemInstance item)
    {
        if (item == null || item.IsEmpty) return false;

        Item = item;
        _icon.sprite = Item.Definition.Icon;
        _icon.preserveAspect = true;
        _icon.enabled = true;
        return true;
    }

    public virtual ItemInstance RemoveItem()
    {
        if (Item == null) return null;

        var old = Item;
        Item = null;
        _icon.enabled = false;
        
        return old;
    }
}
