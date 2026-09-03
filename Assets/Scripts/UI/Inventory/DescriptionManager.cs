using TMPro;
using UnityEngine;

public class DescriptionManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _mainText;
    [SerializeField] GameObject _buttons;
    [SerializeField] GameObject _equipButton;
    [SerializeField] GameObject _unequipButton;
    [SerializeField] GameObject _removeButton;

    public bool IsActive
    {
        get => gameObject.activeInHierarchy;
        set => gameObject.SetActive(value);
    }

    private void Start()
        => IsActive = false;

    void ShowButtons(InventorySlot inventorySlot)
    {
        if (inventorySlot.IsEquippable)
        {
            if (inventorySlot.IsEquipped)
            {
                _equipButton.SetActive(false);
                _unequipButton.SetActive(true);
            }
            else
            {
                _equipButton.SetActive(true);
                _unequipButton.SetActive(false);
            }
        }
        else
        {
            _equipButton.SetActive(false);
            _unequipButton.SetActive(false);
        }

        if (inventorySlot.IsRemovable)
            _removeButton.SetActive(true);
        else
            _removeButton.SetActive(false);
    }

    void HideButtons()
    {
        _equipButton.SetActive(false);
        _unequipButton.SetActive(false);
        _removeButton.SetActive(false);
    }

    public void Show(InventorySlot inventorySlot)
    {
        IsActive = true;
        _mainText.text = inventorySlot.Item.Description;

        ShowButtons(inventorySlot);
    }

    public void Hide()
    {
        IsActive = false;
        _mainText.text = null;

        HideButtons();
    }
}
