using TMPro;
using UnityEngine;

public class DescriptionView : MonoBehaviour
{
    [SerializeField] InventoryCell _cell;
    [SerializeField] TextMeshProUGUI _header;
    [SerializeField] TextMeshProUGUI _extra;
    [SerializeField] TextMeshProUGUI _description;
    [Header("Listens to")]
    [SerializeField] GameobjectGameEvent _selectableChanged;

    private void OnEnable()
    {
        _selectableChanged.AddListener(OnCellSelected);
    }
    private void OnDisable()
    {
        _selectableChanged.RemoveListener(OnCellSelected);
    }

    public void Show(ItemInstance item)
    {
        _cell.SetItem(item);
        _header.text = item.Definition.Name;
        _description.text = item.Description;
    }

    void OnCellSelected(GameObject game)
    {
        Show(game.GetComponent<InventorySlot>().Item);
    }
}
