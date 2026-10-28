using Recalled.Core;
using Recalled.Gameplay.Movement;
using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Movementable))]
    public sealed class Sprint : MonoBehaviour
    {
        [SerializeField] ValueProviderConfig _config;
        [SerializeField] HealthValueVariable _intVariable;
        [SerializeField, Min(0)] int _usage;
        [SerializeField, Min(0)] int _restore;
        [SerializeField, Min(0)] float _deltaBetween;
        [SerializeField, Min(0)] float _speedMultiplier;

        Movementable _movementable;
        ValueResource _valueResource;
        bool _isActive;

        public bool IsFull => _valueResource.IsFull;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive == value)
                    return;

                if (value)
                    _movementable.AddSpeedCoef(_speedMultiplier);
                else
                    _movementable.RemoveSpeedCoef(_speedMultiplier);

                _isActive = value;
            }
        }

        private void Awake()
        {
            _movementable = GetComponent<Movementable>();
            _valueResource = new(_config);

            if (_intVariable != null)
                _intVariable.Value = new(_valueResource.Current, _valueResource.MaxValue);
        }

        private void OnEnable()
        {
            if (_intVariable)
                _valueResource.ValueApplied += SetVariableValue;
        }
        private void OnDisable()
        {
            if (_intVariable)
                _valueResource.ValueApplied -= SetVariableValue;
        }
        void SetVariableValue(int _, int newVal) => _intVariable.Value = _intVariable.Value.WithCurrent(_valueResource.Current);

        public bool AskFor(int amount)
        {
            if (!_valueResource.Has(amount))
                return false;

            _valueResource.Consume(amount);
            return true;
        }

        public void Restore(int quantity)
        {
            if (quantity < 0) throw new ArgumentException();

            _valueResource.Replenish(quantity);
        }

        float _elapsed;
        private void FixedUpdate()
        {
            _elapsed += Time.deltaTime;
            if (_elapsed < _deltaBetween)
                return;

            _elapsed = 0;
            if (IsActive)
            {
                if (_valueResource.Consume(_usage) == 0)
                    IsActive = false;
            }
            else _valueResource.Replenish(_restore);
        }
    }
}
