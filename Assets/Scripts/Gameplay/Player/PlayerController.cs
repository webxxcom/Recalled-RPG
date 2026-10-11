using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Health))]
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
        Health _healthProvider;

        void Awake()
        {
            _healthProvider = GetComponent<Health>();

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
