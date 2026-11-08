using Recalled.Gameplay;
using TMPro;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    [RequireComponent(typeof(PrefabPopulator))]
    internal sealed class StatsView : MonoBehaviour
    {
        [SerializeField] StatsAggregatorAnchor _statsAggregatorAnchor;

        PrefabPopulator _populator;

        private void Awake()
        {
            _populator = GetComponent<PrefabPopulator>();
        }

        private void OnEnable()
        {
            _statsAggregatorAnchor.Provided += OnAggregatorProvided;

            if (_statsAggregatorAnchor.IsSet) OnAggregatorProvided(_statsAggregatorAnchor.Value);
        }
        private void OnDisable()
        {
            _statsAggregatorAnchor.Provided -= OnAggregatorProvided;
        }

        void OnAggregatorProvided(StatsAggregator statsAggregator)
        {
            statsAggregator.Changed += UpdateStats;

            UpdateStats();
        }

        void UpdateStats()
        {
            _populator.DestroyAll();
            foreach (var stat in _statsAggregatorAnchor.Value.Stats)
            {
                var statText = _populator.Add().GetComponent<TMP_Text>();

                statText.text = $"{stat.Key.name}: {stat.Value}";
            }
        }
    }
}
