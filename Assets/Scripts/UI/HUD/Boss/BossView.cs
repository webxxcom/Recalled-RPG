using Recalled.Core;
using Recalled.Gameplay;
using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(PrefabPopulator))]
    public class BossView : MonoBehaviour
    {
        [SerializeField] HealthVariableGameEvent _variableEventChannel;
        PrefabPopulator _populator;

        private void Awake()
        {
            _populator = GetComponent<PrefabPopulator>();
        }

        private void OnEnable() => _variableEventChannel.EventRaised += AddBar;
        private void OnDisable() =>  _variableEventChannel.EventRaised -= AddBar;

        void AddBar(HealthValueVariable variable)
        {
            var variableBinder = _populator.Add().GetComponent<BossBar>();
            
            variableBinder.Init(variable);
        }
    }
}
