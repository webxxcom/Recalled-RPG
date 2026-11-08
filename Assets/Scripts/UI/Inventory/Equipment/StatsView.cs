using Recalled.Systems.Inventory.Stats;
using TMPro;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    [RequireComponent(typeof(PrefabPopulator))]
    internal sealed class StatsView : MonoBehaviour
    {
        PrefabPopulator _populator;

        private void Awake() => _populator = GetComponent<PrefabPopulator>();

        public void UpdateStats(string[] stats)
        {
            _populator.DestroyAll();
            foreach (var stat in stats)
            {
                var statText = _populator.Add().GetComponent<TMP_Text>();

                statText.text = stat;
            }
        }
    }
}
