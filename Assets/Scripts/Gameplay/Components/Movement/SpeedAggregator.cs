using System.Linq;

public class SpeedAggregator : ValueAggregator<float>
{
    public override float Get()
        => Values.Aggregate(1f, (acc, v) => acc * v);
}
