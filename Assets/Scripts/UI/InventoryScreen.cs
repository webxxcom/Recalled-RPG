using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public sealed class InventoryManager : UIScreen
{
    [SerializeField] InventorySO _inventory;

    [Header("UI")]
    [SerializeField] GameObject _basicItemsInventoryGrid;
    [SerializeField] InventorySlot _inventoryItemPrefab;
    [SerializeField] InventorySlot _swordInventoryItem;
    [SerializeField] InventorySlot _armorInventoryItem;
    [SerializeField] InventorySlot _bootsInventoryItem;
    [SerializeField] Highlighter _highlighter;

    [Header("Listens to")]
    [SerializeField] GameobjectGameEvent OnUIElementSelected;
    [SerializeField] VoidGameEvent OnUIElementDeselected;

    DescriptionManager _descriptionManager;
    InventorySlot _selectedInventorySlot;
    readonly List<InventorySlot> _createdInventorySlots = new();

    public event Action OnEquippedItems;

    protected override void Awake()
    {
        base.Awake();

        _descriptionManager = GetComponentInChildren<DescriptionManager>();
    }

    void OnEnable()
    {
        OnUIElementSelected.OnEventRaised += ItemSelected;
        OnUIElementDeselected.OnEventRaised += ItemDeselected;
        _inventory.OnItemsChanged += Open;
    }

    void OnDisable()
    {
        OnUIElementSelected.OnEventRaised -= ItemSelected;
        OnUIElementDeselected.OnEventRaised -= ItemDeselected;
        _inventory.OnItemsChanged -= Open;
    }

    void CreateGeneralItemSlot(ItemInstance itemInstance)
    {
        InventorySlot inventoryItem = Instantiate(_inventoryItemPrefab, _basicItemsInventoryGrid.transform);

        inventoryItem.Initialize(itemInstance);
        _createdInventorySlots.Add(inventoryItem);
    }

    void RefreshGeneralSlots()
    {
        DeleteGeneralSlots();
        _inventory.Items.ForEach(CreateGeneralItemSlot);
    }

    void DeleteGeneralSlots()
    {
        _createdInventorySlots.ForEach(ii => Destroy(ii.gameObject));
        _createdInventorySlots.Clear();
    }

    void RefreshEquipSlots()
    {
        if (_inventory.Sword != null) _swordInventoryItem.Initialize(_inventory.Sword, false, true);
        else _swordInventoryItem.Absent();

        if (_inventory.Armor != null) _armorInventoryItem.Initialize(_inventory.Armor, false, true);
        else _armorInventoryItem.Absent();

        if (_inventory.Boots != null) _bootsInventoryItem.Initialize(_inventory.Boots, false, true);
        else _bootsInventoryItem.Absent();
    }

    public override void Open()
    {
        RefreshGeneralSlots();
        RefreshEquipSlots();
        ItemDeselected();
    }

    public override void Close()
    {
        DeleteGeneralSlots();
        ItemDeselected();
    }

    public void ItemSelected(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out InventorySlot inventorySlot))
        {
            _selectedInventorySlot = inventorySlot;

            if (inventorySlot.Item == null)
                return;

            _highlighter.Show(gameObject);
            _descriptionManager.Show(inventorySlot);
        }
    }

    public void ItemDeselected()
    {
        _selectedInventorySlot = null;

        _highlighter.Hide();
        _descriptionManager.Hide();
    }

    public void OnRemoveButtonClick() => RemoveItem(_selectedInventorySlot);
    public void OnEquipButtonClick() => EquipItem(_selectedInventorySlot);
    public void OnUnequipButtonClick() => UnequipItem(_selectedInventorySlot);

    void RemoveItem(InventorySlot inventorySlot)
    {
        Destroy(inventorySlot.gameObject);
        _inventory.Remove(inventorySlot.Item);
    }

    void UnequipItem(InventorySlot inventorySlot)
    {
        if (inventorySlot.Item is IEquippable equippable)
        {
            ItemInstance unequipped = equippable.Unequip(_inventory);

            OnEquippedItems?.Invoke();
        }
    }

    void EquipItem(InventorySlot inventorySlot)
    {
        if (inventorySlot.Item is IEquippable equippable)
        {
            ItemInstance replaced = equippable.Equip(_inventory);

            OnEquippedItems?.Invoke();
        }
    }
}
