using Recalled.Gameplay;
using UnityEngine;

namespace Recalled.UI
{
    public sealed class VariableBarBinder : MonoBehaviour
    {
        [SerializeField] BarScript _bar;
        [SerializeField] HealthValueVariable _healthVariable;
        [SerializeField] bool _destroyOnMin = true;

        private void Awake()
        {
            if (_bar == null || _healthVariable == null)
                throw new MissingReferenceException();
        }

        private void Start() => OnValueChanged(_healthVariable.Value);
        private void OnEnable() => _healthVariable.Subscribe(OnValueChanged);
        private void OnDisable() => _healthVariable.Unsubscribe(OnValueChanged);

        void OnValueChanged(HealthValue hv)
        {
            _bar.Set(hv.Normalized);

            if (hv.Current == 0 && _destroyOnMin)
                Destroy(gameObject, 2);
        }
    }
}
