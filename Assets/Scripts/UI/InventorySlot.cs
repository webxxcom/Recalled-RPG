using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public sealed class InventorySlot : InventoryCell
{
    [SerializeField] Selectable _selectable;
    [SerializeField] TextMeshProUGUI _countText;

    public override bool SetItem(ItemInstance item)
    {
        if (base.SetItem(item))
        {
            _selectable.enabled = true;

            if (item.Definition.IsStackable)
            {
                _countText.enabled = true;
                _countText.text = item.Count.ToString();
            }
            else
                _countText.enabled = false;

            return true;
        }
        return false;
    }

    public override ItemInstance RemoveItem()
    {
        _selectable.enabled = false;
        _countText.enabled = false;

        return base.RemoveItem();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_selectable == null) _selectable = GetComponent<Selectable>();
    }
#endif
}
