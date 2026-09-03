using UnityEngine;

[CreateAssetMenu(menuName = "InventorySO Items/Boots")]
public class BootsDefinition : ItemDefinition
{
    [field: SerializeField] public float SpeedMultiplier { get; private set; }
    [field: SerializeField] public float Protection { get; private set; }

    public override ItemInstance CreateInstance() => new Boots(this);
}
