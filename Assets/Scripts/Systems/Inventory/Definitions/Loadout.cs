using System.Linq;
using UnityEngine;

namespace Recalled.Systems.Inventory
{
    [CreateAssetMenu(menuName = "Bag/Loadout")]
    public sealed class Loadout : ScriptableObject
    {
        [SerializeField] int _capacity;
        [SerializeField] ItemInstance[] _items;

        public int Capacity => _capacity;
        public ItemInstance[] Items => _items;

        private void OnValidate()
        {
            if (_items.Any(i => i.Definition == null))
                Debug.LogWarning($"{nameof(Loadout)} can't have null {nameof(ItemDefinition)}s");
        }
    }
}
