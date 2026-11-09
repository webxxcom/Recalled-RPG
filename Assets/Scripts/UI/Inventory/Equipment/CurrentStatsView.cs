using Recalled.Core;
using Recalled.Gameplay;
using Recalled.Systems.Inventory.Stats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.UI.Inventory
{
    [RequireComponent(typeof(StatsView))]
    internal sealed class CurrentStatsView : UiView<StatsAggregator>
    {
        [SerializeField] StatsAggregatorAnchor _statsAggregatorAnchor;

        StatsView _statsView;
        protected override RuntimeAnchor<StatsAggregator> Anchor => _statsAggregatorAnchor;
        private void Awake() => _statsView = GetComponent<StatsView>();

        string OutputStatView(KeyValuePair<StatDefinition, float> keyValue)
            => $"{keyValue.Key.name}: {keyValue.Value}"; 
        protected override void Refresh()
        {
            var statsAggregator = _statsAggregatorAnchor.Value;

            _statsView.UpdateStats(statsAggregator.Stats.Select(OutputStatView).ToArray());
        }
    }
}
