using Recalled.Core;
using Recalled.Systems.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(InventoryHolder))]
    sealed class QuickSlotsHolder : MonoBehaviour
    {
        [SerializeField] List<ItemDefinition> _loadout;

        [Header("Sets")]
        [SerializeField] QuickSlotsAnchor _slotsAnchor;

        [Header("Raises")]
        [SerializeField] VoidGameEvent _quickSlotsChanged;

        QuickSlots _quickSlots;

        private void Awake() => _quickSlots = new(GetComponent<InventoryHolder>().Inventory);
        private void Start()
        {
            int i = 0;
            foreach (var item in _loadout)
                _quickSlots.Put(i++, item);

            _slotsAnchor.Provide(_quickSlots);
        }
        private void OnEnable() =>  _quickSlots.Changed += RaiseChanged;
        private void OnDisable() => _quickSlots.Changed -= RaiseChanged;
        private void OnDestroy() => _slotsAnchor.Withdraw(_quickSlots);

        void RaiseChanged() => _quickSlotsChanged.Invoke();

        void OnQuickSlots(InputValue value)
        {
            var vec2 = value.Get<Vector2>();

            int i = -1;
            if (vec2 == Vector2.left) i = 0;
            else if (vec2 == Vector2.up) i = 1;
            else if (vec2 == Vector2.right) i = 2;
            else if (vec2 == Vector2.down) i = 3;

            if (i == -1)
                return;

            _quickSlots.Use(i, gameObject);
        }
    }
}
