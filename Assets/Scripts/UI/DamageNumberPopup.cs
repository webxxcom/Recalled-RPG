using System;
using UnityEngine;

namespace Recalled.UI
{
    public class DamageNumberPopup : HealthReactor
    {
        [SerializeField] PopupWorldText _damagePopup;

        private void Awake()
        {
            if (_damagePopup == null)
                throw new ArgumentNullException();
        }

        protected override void OnHpChanged(DamageInfo di)
        {
            Instantiate(_damagePopup, _health.Hurtbox.bounds.center, Quaternion.identity)
                .Init(di.Amount.ToString());
        }
    }
}
