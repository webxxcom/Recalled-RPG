using Recalled.Core;
using Recalled.Gameplay;
using System;
using TMPro;
using UnityEngine;

namespace Recalled.UI
{
    public class BossBar : MonoBehaviour
    {
        [SerializeField] BarScript _bar;
        [SerializeField] TMP_Text _text;

        HealthValueVariable _healthVariable;

        private void OnDestroy() => _healthVariable.Unsubscribe(OnValueChanged);

        public void Init(HealthValueVariable variable)
        {
            if (variable == null)
                throw new ArgumentNullException();

            _healthVariable = variable;
            _text.text = _healthVariable.name;
            _healthVariable.Subscribe(OnValueChanged);
        }

        void OnValueChanged(HealthValue value)
        {
            _bar.Set(value.Normalized);

            if (value.Current == 0)
                Destroy(gameObject, 2);
        }
    }
}
