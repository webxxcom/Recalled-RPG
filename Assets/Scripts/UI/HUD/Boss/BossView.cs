using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(PrefabPopulator))]
    public class BossView : MonoBehaviour
    {
        [SerializeField] IntVariableGameEvent _variableEventChannel;
        PrefabPopulator _populator;

        private void Awake()
        {
            _populator = GetComponent<PrefabPopulator>();
        }

        private void OnEnable()
        {
            _variableEventChannel.EventRaised += AddBar;
        }
        private void OnDisable()
        {
            _variableEventChannel.EventRaised -= AddBar;
        }

        void AddBar(IntVariable variable)
        {
            var bossBar = _populator.Add().GetComponent<BossBar>();
            
            bossBar.Init(variable);
        }
    }
}
