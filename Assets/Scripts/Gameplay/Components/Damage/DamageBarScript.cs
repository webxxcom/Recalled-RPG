using Recalled.Core;
using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class DamageBarScript : MonoBehaviour
    {
        [SerializeField] BarScript _barScript;
        [SerializeField] BarSwitcher _switcher;
        [SerializeField] Health _health;

        private void Awake()
        {
            if (_barScript == null || _health == null || _switcher == null)
                throw new MissingReferenceException();
        }

        void OnEnable()
        {
            _health.HpChanged += SetBar;
            _health.Died += SwitchBars;
        }
        void OnDisable()
        {
            _health.HpChanged -= SetBar;
            _health.Died -= SwitchBars;
        }

        void SetBar(DamageInfo _) => _barScript.Set((float)_health.Current / _health.MaxValue);
        void SwitchBars(DamageInfo _) => _switcher.SwitchBars();
    }
}
