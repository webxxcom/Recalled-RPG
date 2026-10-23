using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.Gameplay.Combat
{
    [RequireComponent(typeof(AttackStrategy))]
    public abstract class EntityAttack : MonoBehaviour
    {
        [SerializeField] protected EntityAnimationController _animationController;
        [SerializeField] protected List<AttackStrategy> _attackStrategies;

        protected float _elapsedLastAttack;

        public event Action OnAttackStarted;
        public event Action OnAttackFinished;

        bool IsActive => enabled;

        AttackStrategy _currentAttack;
        AttackContext _attackContext;
        protected void Attack(AttackStrategy attackStrategy, AttackContext attackContext)
        {
            _currentAttack = attackStrategy;
            _attackContext = attackContext;

            _animationController.PlayAttack(attackStrategy.AnimatorHash);
        }

        public void StartAttack()
        {
            _currentAttack.StartExecuting(_attackContext);

            OnAttackStarted?.Invoke();
        }

        public void ProcessAttack(float normalizedTime)
        {
            if (!IsActive)
                return;

            _currentAttack.ProcessState(normalizedTime, _attackContext);
        }

        public void FinishAttack()
        {
            _currentAttack.FinishExecuting(_attackContext);

            OnAttackFinished?.Invoke();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_attackStrategies == null || _attackStrategies.Count == 0)
                GetComponents(_attackStrategies);
        }

#endif
    }
}
