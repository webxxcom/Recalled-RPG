using Recalled.Systems.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public sealed class InventorySlotView : InventoryCell
{
    [SerializeField] Selectable _selectable;
    [SerializeField] TextMeshProUGUI _countText;

    public override void SetItem(ItemInstance instance)
    {
        base.SetItem(instance);

        _selectable.enabled = true;
        if (instance.Definition.IsStackable)
        {
            _countText.enabled = true;
            _countText.text = instance.Count.ToString();
        }
        else
            _countText.enabled = false;
    }

    public override void RemoveItem()
    {
        _selectable.enabled = false;
        _countText.enabled = false;

        base.RemoveItem();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_selectable == null) _selectable = GetComponent<Selectable>();
    }
#endif
}
