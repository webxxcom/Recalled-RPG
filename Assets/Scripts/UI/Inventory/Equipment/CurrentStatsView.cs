using Recalled.Gameplay;
using Recalled.Systems.Inventory.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    [RequireComponent(typeof(StatsView))]
    internal sealed class CurrentStatsView : MonoBehaviour
    {
        [SerializeField] StatsAggregatorAnchor _statsAggregatorAnchor;

        StatsView _statsView;

        private void Awake() => _statsView = GetComponent<StatsView>();

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
            statsAggregator.Changed += Refresh;

            Refresh();
        }

        string OutputStatView(KeyValuePair<StatDefinition, float> keyValue)
            => $"{keyValue.Key.name}: {keyValue.Value}"; 
        public void Refresh()
        {
            _statsView.UpdateStats(_statsAggregatorAnchor.Value.Stats.Select(OutputStatView).ToArray());
        }
    }
}
