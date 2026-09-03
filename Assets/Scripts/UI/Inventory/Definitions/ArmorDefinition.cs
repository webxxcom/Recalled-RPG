using UnityEngine;

[CreateAssetMenu(menuName = "InventorySO Items/Armor")]
public class ArmorDefinition : ItemDefinition
{
    [field: SerializeField] public float Protection { get; private set; }
    [field: SerializeField] public float Weight { get; private set; }

    public override ItemInstance CreateInstance() => new Armor(this);
}
