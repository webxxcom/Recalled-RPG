using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Animator _animator;

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
    }
}
