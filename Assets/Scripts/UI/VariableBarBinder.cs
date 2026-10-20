using Recalled.Core;
using UnityEngine;

namespace Recalled.UI
{
    public sealed class VariableBarBinder : MonoBehaviour
    {
        [SerializeField] BarScript _bar;
        [SerializeField] IntVariable _intVariable;

        int _max;

        private void Awake()
        {
            if (_bar == null || _intVariable == null)
                throw new MissingReferenceException();
        }

        private void Start() => _max = _intVariable.Value;
        private void OnEnable() => _intVariable.Subscribe(OnValueChanged);
        private void OnDisable() => _intVariable.Unsubscribe(OnValueChanged);

        void OnValueChanged(int value)
        {
            _bar.Set(value / (float)_max);

            if (value == 0)
                Destroy(gameObject, 2);
        }
    }
}
