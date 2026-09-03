using UnityEngine;

[CreateAssetMenu(menuName = "InventorySO Items/Sword")]
public class SwordDefinition : ItemDefinition
{
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float KnockbackPower { get; private set; }
    [field: SerializeField] public float Weight { get; private set; }
    [field: SerializeField] public float ReloadTime { get; private set; }

    public override ItemInstance CreateInstance() => new Sword(this);
}
