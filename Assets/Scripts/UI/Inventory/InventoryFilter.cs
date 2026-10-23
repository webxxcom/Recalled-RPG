using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    [RequireComponent(typeof(Toggleable))]
    public sealed class InventoryFilter : MonoBehaviour
    {
        [SerializeField] ItemCategory _category;
        [SerializeField] InventorySlotsView _view;

        IToggleable _toggleable;

        private void Awake() => _toggleable = GetComponent<Toggleable>();

        private void OnEnable()
        {
            _toggleable.StateChanged += OnActivated;
        }
        private void OnDisable()
        {
            _toggleable.StateChanged -= OnActivated;
        }

        void OnActivated(bool isActive)
        {
            if (isActive) _view.Filter(_category);
            else _view.Filter(ItemCategory.Any);
        }
    }
}
