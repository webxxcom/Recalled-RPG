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

        _descriptionManager = Utils.FindOrThrow(FindAnyObjectByType<DescriptionManager>);
    }

    void OnEnable()
    {
        OnUIElementSelected.OnEventRaised += ItemSelected;
        OnUIElementDeselected.OnEventRaised += ItemDeselected;
    }

    void OnDisable()
    {
        OnUIElementSelected.OnEventRaised -= ItemSelected;
        OnUIElementDeselected.OnEventRaised -= ItemDeselected;
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
    }

    public override void Close()
    {
        DeleteGeneralSlots();
        _highlighter.Hide();
    }

    public void ItemSelected(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out InventorySlot inventorySlot))
        {
            _selectedInventorySlot = inventorySlot;

            if (inventorySlot.Item == null)
                return;

            if (gameObject.TryGetComponent(out InventorySlot _))
                _highlighter.Show(gameObject);
            _descriptionManager.Show(inventorySlot);
        }
    }

    public void ItemDeselected()
    {
        _selectedInventorySlot = _createdInventorySlots.FirstOrDefault();

        _highlighter.Hide();
        _descriptionManager.Hide();
    }

    public void OnRemoveButtonClick() => RemoveItem(_selectedInventorySlot);
    public void OnEquipButtonClick() => EquipItem(_selectedInventorySlot);
    public void OnUnequipButtonClick() => UnequipItem(_selectedInventorySlot);

    void RemoveItem(InventorySlot inventorySlot)
    {
        _inventory.Remove(inventorySlot.Item);
        _createdInventorySlots.Remove(inventorySlot);
        Destroy(inventorySlot.gameObject);
        ItemDeselected();
    }

    void UnequipItem(InventorySlot inventorySlot)
    {
        if (inventorySlot.Item is IEquippable equippable)
        {
            ItemInstance unequipped = equippable.Unequip(_inventory);

            CreateGeneralItemSlot(unequipped);
            ItemDeselected();
            RefreshEquipSlots();
            OnEquippedItems?.Invoke();
        }
    }

    void EquipItem(InventorySlot inventorySlot)
    {
        if (inventorySlot.Item is IEquippable equippable)
        {
            ItemInstance replaced = equippable.Equip(_inventory);

            if (replaced == null)
                RemoveItem(inventorySlot);

            RefreshEquipSlots();
            RefreshGeneralSlots();
            _descriptionManager.Hide();
            _highlighter.Hide();
            OnEquippedItems?.Invoke();
        }
    }
}
