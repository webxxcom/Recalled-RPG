using UnityEngine;

namespace Recalled.Systems.Inventory
{
    [CreateAssetMenu(menuName = "Bag/Items/Boots")]
    public class BootsDefinition : ItemDefinition
    {
        [field: SerializeField] public float SpeedMultiplier { get; private set; }
        [field: SerializeField] public float Protection { get; private set; }

        public override ItemInstance CreateInstance(int count = 1) => new Boots(this);
    }
}
