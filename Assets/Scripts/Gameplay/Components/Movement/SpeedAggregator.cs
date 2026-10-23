using System.Linq;
using Recalled.Core;

namespace Recalled.Gameplay
{
    public sealed class SpeedAggregator : ValueAggregator<float>
    {
        public override float Get()
            => Values.Aggregate(1f, (acc, v) => acc * v);
    }
}
