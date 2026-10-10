using UnityEngine;

namespace Recalled.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        [Header("Broadcasts to")]
        [SerializeField] DamageInfoGameEvent OnHpChangedChannel;
        [SerializeField] DamageInfoGameEvent OnDeathChannel;

        bool _isArmed;
        public bool IsArmed
        {
            get => _isArmed;
            private set
            {
                _isArmed = value;
                _animator.SetBool(AnimatorParameters.IsArmedHash, value);
            }
        }
        HealthResource _healthProvider;

        void Awake()
        {
            _healthProvider = GetComponentInChildren<HealthResource>();

            IsArmed = true;
        }

        void OnEnable()
        {
            _healthProvider.HpChanged += OnHpChangedGameEvent;
            _healthProvider.Died += OnDiedGameEvent;
        }

        void OnDisable()
        {
            _healthProvider.HpChanged -= OnHpChangedGameEvent;
            _healthProvider.Died -= OnDiedGameEvent;
        }

        void OnHpChangedGameEvent(DamageInfo damageInfo) => OnHpChangedChannel.Invoke(damageInfo);
        void OnDiedGameEvent(DamageInfo damageInfo) => OnDeathChannel.Invoke(damageInfo);
    }
}
