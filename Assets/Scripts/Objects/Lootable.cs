using UnityEngine;

public class Lootable : MonoBehaviour
{
    [SerializeField] InventorySO _inventory;
    [SerializeField] LootTable _lootTable;
    [SerializeField] PopupWorldText _rejectItemText;

    public bool LootItem()
    {
        ItemInstance item = _lootTable.GetItem().CreateInstance();

        if (_inventory.AddItem(item))
            return true;
        else
        {
            RejectItem(item);
            return false;
        }
    }

    void RejectItem(ItemInstance item)
    {
        PopupWorldText pwt = Instantiate(_rejectItemText, transform.position, Quaternion.identity);
        pwt.Init($"'{item.Definition.Name}'\nrejected..");
    }
}
