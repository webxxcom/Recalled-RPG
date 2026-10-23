using Recalled.Gameplay.Combat;
using UnityEngine;

namespace Recalled.Gameplay
{
    [SelectionBase, DisallowMultipleComponent]
    public sealed class TrainingDummyAnimation : MonoBehaviour
    {
        private static readonly int DamageHardHash = Animator.StringToHash("HurtHard");
        private static readonly int DamageMidHash = Animator.StringToHash("HurtMid");
        private static readonly int DamageLightHash = Animator.StringToHash("HurtLight");

        [SerializeField] Animator _animator;

        public void HpChanged(DamageInfo damageInfo)
        {
            if (damageInfo.Amount > 0)
            {
                if (damageInfo.Amount <= 10)
                    _animator.SetTrigger(DamageLightHash);
                else if (damageInfo.Amount <= 25)
                    _animator.SetTrigger(DamageMidHash);
                else
                    _animator.SetTrigger(DamageHardHash);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
        }
#endif
    }
}
