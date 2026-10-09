using UnityEngine;

namespace Recalled.Systems.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Loadout")]
    public sealed class Loadout : ScriptableObject
    {
        [SerializeField] int _capacity;
        [SerializeField] ItemInstance[] _items;

        public int Capacity => _capacity;
        public ItemInstance[] Items => _items;
    }
}
