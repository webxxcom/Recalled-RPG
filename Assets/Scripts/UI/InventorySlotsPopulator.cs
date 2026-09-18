using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventorySlotsView))]
public class InventorySlotsPopulator : MonoBehaviour
{
    [SerializeField] InventorySlot _inventorySlotPrefab;
    [SerializeField] GameObject _parent;

    InventorySlotsView _slotsView;
    readonly List<InventoryCell> _createdSlots = new();
    bool _isDirty;

    private void Awake()
    {
        _slotsView = GetComponent<InventorySlotsView>();

        PopulateView();
    }

    private void OnEnable()
    {
        _isDirty = true;
    }

    void PopulateView()
    {
        for (int i = 0; i < _slotsView.List.Items.Count; i++)
        {
            InventoryCell slot = Instantiate(_inventorySlotPrefab, _parent.transform);

            _createdSlots.Add(slot);
        }
    }

    private void Update()
    {
        if (_isDirty)
        {
            EventSystem.current.SetSelectedGameObject(_createdSlots[0].gameObject);
            _isDirty = false;
        }
    }
}
