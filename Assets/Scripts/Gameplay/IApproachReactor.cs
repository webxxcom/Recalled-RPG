namespace Recalled.Gameplay
{
    public interface IApproachReactor
    {
        void ReactToApproach(IApproachable approachable);
        void ReactToRetreat(IApproachable approachable);
    }
}
