using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] Image _icon;

    public ItemInstance Item { get; private set; }
    public bool HasItem => Item != null;

    public virtual void SetItem(ItemInstance item)
    {
        if (item == null) return;

        Item = item;
        _icon.sprite = Item.Definition.Icon;
        _icon.preserveAspect = true;
        _icon.enabled = true;
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
