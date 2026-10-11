using Recalled.Gameplay;
using System;
using UnityEngine;

namespace Recalled.UI
{
    public class DamagePopupSpawner : MonoBehaviour
    {
        [SerializeField] PopupWorldText _damagePopup;

        private void Awake()
        {
            if (_damagePopup == null)
                throw new ArgumentNullException();
        }

        public void Spawn(DamageInfo di)
            => Instantiate(_damagePopup).Init(di.Target.Hurtbox.bounds.center, di.Amount.ToString());
    }
}
