using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class InventorySlot : MonoBehaviour
{
    [SerializeField] Sprite _absentSprite;

    public ItemInstance Item { get; private set; }
    public TextMeshProUGUI CountText { get; private set; }
    public bool IsRemovable { get; private set; }
    public bool IsEquippable => Item is IEquippable;
    public bool IsEquipped { get; set; }

    Image _image;
    Button _button;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        CountText = Utils.FindOrThrow(GetComponentInChildren<TextMeshProUGUI>);
    }

    public void Absent()
    {
        _button.enabled = false;
        IsRemovable = false;
        IsEquipped = true;
        _image.sprite = _absentSprite;
        CountText.text = null;
    }

    public void Initialize(ItemInstance itemInstance, bool isRemovable = true, bool isEquipped = false)
    {
        Item = itemInstance;

        _button.enabled = true;
        IsRemovable = isRemovable;
        IsEquipped = isEquipped;
        _image.sprite = itemInstance.Definition.Icon;
        CountText.text = itemInstance.Count != 1 ? itemInstance.Count.ToString() : null;
    }
}
