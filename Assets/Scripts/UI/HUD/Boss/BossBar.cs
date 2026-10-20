using Recalled.Core;
using System;
using TMPro;
using UnityEngine;

namespace Recalled.UI
{
    public class BossBar : MonoBehaviour
    {
        [SerializeField] BarScript _bar;
        [SerializeField] TMP_Text _text;

        IntVariable _intVariable;
        int _max;

        private void OnDestroy() => _intVariable.Unsubscribe(OnValueChanged);

        public void Init(IntVariable variable)
        {
            if (variable == null)
                throw new ArgumentNullException();

            _intVariable = variable;
            _max = _intVariable.Value;
            _text.text = _intVariable.name;
            _intVariable.Subscribe(OnValueChanged);
        }

        void OnValueChanged(int value)
        {
            _bar.Set(value / (float)_max);

            if (value == 0)
                Destroy(gameObject, 2);
        }
    }
}
