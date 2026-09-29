using UnityEngine;

namespace Recalled.UI
{
    public class DamageNumberPopup : HealthReactor
    {
        [SerializeField] PopupWorldText _damagePopup;

        private void Awake()
        {
            if (_damagePopup == null)
            {
                Debug.Log($"{gameObject.name} doesn't have {nameof(_damagePopup)} assigned");
                enabled = false;
            }
        }

        protected override void OnHpChange(DamageInfo di)
        {
            Instantiate(_damagePopup, _health.Hurtbox.bounds.center, Quaternion.identity)
                .Init(di.Amount.ToString());
        }
    }
}
