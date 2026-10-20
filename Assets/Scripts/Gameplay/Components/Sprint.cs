using Recalled.Core;
using Recalled.Gameplay.Movement;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Movementable))]
    public sealed class Sprint : MonoBehaviour
    {
        [SerializeField] ValueProviderConfig _config;
        [SerializeField] IntVariable _intVariable;
        [SerializeField] int _usage;
        [SerializeField] int _restore;
        [SerializeField] float _speedMultiplier;

        Movementable _movementable;
        ValueResource _valueResource;
        bool _isActive;

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

            if (_intVariable != null) _intVariable.Value = _valueResource.Current;
        }

        private void OnEnable()
        {
            if (_intVariable) _valueResource.ValueApplied += SetVariableValue;
        }
        private void OnDisable()
        {
            if (_intVariable) _valueResource.ValueApplied -= SetVariableValue;
        }
        void SetVariableValue(int _, int newVal) => _intVariable.Value = newVal;

        public bool AskFor(int amount)
        {
            if (!_valueResource.Has(amount))
                return false;

            _valueResource.Consume(amount);
            return true;
        }

        private void FixedUpdate()
        {
            if (IsActive)
            {
                if (_valueResource.Consume(_usage) == 0)
                    IsActive = false;
            }
            else _valueResource.Replenish(_restore);
        }
    }
}
