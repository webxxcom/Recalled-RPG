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
        {
            Instantiate(_damagePopup, di.Target.Hurtbox.bounds.center, Quaternion.identity)
                .Init(di.Amount.ToString());
        }
    }
}
