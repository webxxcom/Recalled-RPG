using UnityEngine;

[CreateAssetMenu(menuName = "Combat Value/Player")]
public sealed class PlayerCombatData : ScriptableObject
{
    [SerializeField] InventorySO _inventory;
    [SerializeField] MeleeAttackSO _playerMelee;

    public int DealtDamage
        => _inventory.Sword?.Definition.Damage ?? _playerMelee.DealtDamage;

    public float Weight
        => (_inventory.Armor?.Definition.Weight ?? 1)
                * (_inventory.Sword?.Definition.Weight ?? 1)
                / (_inventory.Boots?.Definition.SpeedMultiplier ?? 1);

    public float Protection
        => (_inventory.Armor?.Definition.Protection ?? 1)
                * (_inventory.Boots?.Definition.Protection ?? 1);

    public float KnockbackPower
        => _inventory.Sword?.Definition.KnockbackPower ?? _playerMelee.KnockbackPower;
    public float ReloadTime
        => _inventory.Sword?.Definition.ReloadTime ?? _playerMelee.ReloadTime;
}
