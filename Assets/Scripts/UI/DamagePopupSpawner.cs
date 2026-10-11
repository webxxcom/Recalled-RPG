using Recalled.Gameplay;
using UnityEngine;

namespace Recalled.UI
{
    public class DamagePopupSpawner : MonoBehaviour
    {
        [SerializeField] PopupWorldText _damagePopup;
        [SerializeField] Health _health;

        private void Awake()
        {
            if (_damagePopup == null || _health == null)
                throw new MissingReferenceException();
        }

        void OnEnable() => _health.HpChanged += Spawn;
        void OnDisable() => _health.HpChanged -= Spawn;
        public void Spawn(DamageInfo di)
            => Instantiate(_damagePopup).Init(di.Target.Center, di.Amount.ToString());
    }
}
