using Recalled.Core;
using Recalled.Systems.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay.Inventory
{
    [RequireComponent(typeof(InventoryHolder))]
    sealed class QuickSlotsHolder : MonoBehaviour
    {
        [SerializeField] ItemDefinition[] _loadout;

        [Header("Sets")]
        [SerializeField] QuickSlotsAnchor _slotsAnchor;

        [Header("Raises")]
        [SerializeField] VoidGameEvent _quickSlotsChanged;

        QuickSlots _quickSlots;

        private void Start()
        {
            _quickSlots = new(_loadout, GetComponent<InventoryHolder>().Inventory);

            _quickSlots.Changed += () => _quickSlotsChanged.Invoke();
            _slotsAnchor.Provide(_quickSlots);
        }
        private void OnDestroy() => _slotsAnchor.Withdraw(_quickSlots);

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
        }
    }
}
