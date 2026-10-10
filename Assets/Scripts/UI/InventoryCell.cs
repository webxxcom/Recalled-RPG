using Recalled.Systems.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] Image _icon;

    public ItemInstance ItemInstance { get; private set; }

    public virtual void SetItem(ItemInstance instance)
    {
        ItemInstance = instance;
        _icon.sprite = ItemInstance.Definition.Icon;
        _icon.preserveAspect = true;
        _icon.enabled = true;
    }

    public virtual void RemoveItem()
    {
        ItemInstance = null;
        _icon.enabled = false;
    }
}
