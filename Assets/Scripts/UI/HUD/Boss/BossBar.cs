using Recalled.Core;
using System;
using TMPro;
using UnityEngine;

namespace Recalled.UI
{
    public class BossBar : MonoBehaviour
    {
        [SerializeField] TMP_Text _text;
        [SerializeField] BarScript _bar;

        IntVariable _healthVariable;

        public void Init(IntVariable intVariable)
        {
            if (intVariable == null)
                throw new ArgumentNullException();

            _healthVariable = intVariable;
            _healthVariable.Subscribe(OnValueChanged);
        }

        private void OnDestroy()
        {
            if (_healthVariable != null)
                _healthVariable.Unsubscribe(OnValueChanged);
        }

        void OnValueChanged(int value)
        {
            _text.text = _healthVariable.name;
            _bar.Set(value);
        }
    }
}
