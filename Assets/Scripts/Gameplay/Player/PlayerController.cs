using UnityEngine;

namespace Recalled.Gameplay
{
    public class PlayerController : EntityController
    {
        bool _isArmed;
        public bool IsArmed
        {
            get => _isArmed;
            private set
            {
                _isArmed = value;
                Animator.SetBool(AnimatorParameters.IsArmedHash, value);
            }
        }

        [Header("Broadcasts to")]
        [SerializeField] DamageInfoGameEvent OnHpChangedChannel;
        [SerializeField] DamageInfoGameEvent OnDeathChannel;

        HealthResource _healthProvider;

        protected override void Awake()
        {
            base.Awake();

            _healthProvider = GetComponentInChildren<HealthResource>();

            IsArmed = true;
        }

        void OnEnable()
        {
            _healthProvider.HpChangeApplied += OnHpChangedGameEvent;
            _healthProvider.Died += OnDiedGameEvent;
        }

        void OnDisable()
        {
            _healthProvider.HpChangeApplied -= OnHpChangedGameEvent;
            _healthProvider.Died -= OnDiedGameEvent;
        }

        void OnHpChangedGameEvent(DamageInfo damageInfo) => OnHpChangedChannel.Invoke(damageInfo);
        void OnDiedGameEvent(DamageInfo damageInfo) => OnDeathChannel.Invoke(damageInfo);
    }
}
