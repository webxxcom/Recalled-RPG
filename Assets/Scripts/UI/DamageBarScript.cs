using Recalled.Gameplay;
using Recalled.Gameplay.Combat;
using UnityEngine;

namespace Recalled.UI
{
    public sealed class DamageBarScript : MonoBehaviour
    {
        [SerializeField] Health _health;
        [SerializeField] BarScript _barScript;

        private void Awake()
        {
            if (_health == null)
                throw new MissingReferenceException();
        }

        void OnEnable()
        {
            _health.HpChanged += SetBar;
        }
        void OnDisable()
        {
            _health.HpChanged -= SetBar;
        }

        void SetBar(DamageInfo _) => _barScript.Set((float)_health.Current / _health.MaxValue);
    }
}
