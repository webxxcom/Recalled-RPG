using TMPro;
using UnityEngine;

public class DescriptionView : MonoBehaviour
{
    [SerializeField] InventoryCell _cell;
    [SerializeField] TextMeshProUGUI _header;
    [SerializeField] TextMeshProUGUI _extra;
    [SerializeField] TextMeshProUGUI _description;
    [Header("Reads"), SerializeField] GameObjectRuntimeVariable _currentSelected;

    private void OnEnable()
    {
        _currentSelected.ValueChanged += OnCellSelected;
    }
    private void OnDisable()
    {
        _currentSelected.ValueChanged -= OnCellSelected;
    }

    public void Show(ItemInstance item)
    {
        _cell.SetItem(item);
        _header.text = item.Definition.Name;
        _description.text = item.Description;
    }

    void OnCellSelected(GameObject game)
    {
        if (game != null) Show(game.GetComponent<InventorySlot>().Item);
    }
}
