using UnityEngine;

public class DamageNumberPopup : HealthReactor
{
    [SerializeField] PopupWorldText _damagePopup;
    [SerializeField] SettingsConfig _settings;

    protected override void OnHpChange(DamageInfo di)
    {
        if (_settings.SettingsData._data.showDamageNumbers)
            Instantiate(_damagePopup, _health.Hurtbox.bounds.center, Quaternion.identity)
                .Init(di.Amount.ToString());
    }
}
