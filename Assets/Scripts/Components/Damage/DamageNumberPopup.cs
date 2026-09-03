using UnityEngine;

public class DamageNumberPopup : HealthReactor
{
    [SerializeField] PopupWorldText _damagePopup;

    protected override void OnHpChange(DamageInfo di)
    {
        Instantiate(_damagePopup, _health.Hurtbox.bounds.center, Quaternion.identity)
            .Init(di.Amount.ToString());
    }
}
