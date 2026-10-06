namespace Recalled.Gameplay
{
    public interface IApproachReactor
    {
        public bool enabled { get; }

        void ReactToApproach(Approachable approachable);
        void ReactToRetreat(Approachable approachable);
    }
}
