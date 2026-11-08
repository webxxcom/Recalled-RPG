using Recalled.Systems.Inventory;
using UnityEngine;

namespace Recalled.Gameplay.Inventory
{
    [RequireComponent(typeof(InventoryHolder))]
    internal sealed class EquipmentHolder : MonoBehaviour
    {
        [SerializeField] EquipmentAnchor _equipmentAnchor;

        Equipment _reference;
        public Equipment Reference
        {
            get
            {
                _reference ??= new(GetComponent<InventoryHolder>().Reference);
                return _reference;
            }
        }

        private void Awake() => _equipmentAnchor.Provide(Reference);
        private void OnDestroy() => _equipmentAnchor.Withdraw(Reference);
    }
}
