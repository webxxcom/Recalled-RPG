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
            int i = 0;
            for (; i < stats.Length; i++)
            {
                if (i >= _populator.Created.Count) _populator.Add();

                var statText = _populator.Created[i].GetComponent<TMP_Text>();
                statText.text = stats[i];
            }
            _populator.RemoveAll(i);
        }
    }
}
