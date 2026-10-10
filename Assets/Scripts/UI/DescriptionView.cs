using Recalled.Systems.Inventory;
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

    public void Show(ItemInstance instance)
    {
        _cell.SetItem(instance);
        _header.text = instance.Definition.Name;
        _description.text = instance.Description;
    }

    void OnCellSelected(GameObject game)
    {
        if (game != null) Show(game.GetComponent<InventorySlotView>().ItemInstance);
    }
}
