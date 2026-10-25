using UnityEngine;

namespace Recalled.Systems.Inventory
{
    [CreateAssetMenu(menuName = "Bag/Items/Armor")]
    public class ArmorDefinition : ItemDefinition
    {
        [field: SerializeField] public float Protection { get; private set; }
        [field: SerializeField] public float Weight { get; private set; }

        public override ItemInstance CreateInstance(int count = 1) => new Armor(this);
    }
}
