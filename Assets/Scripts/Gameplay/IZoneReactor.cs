namespace Recalled.Gameplay
{
    public interface IZoneReactor
    {
        // Behavior has it lowercase so we don't have to manually define the property
        public bool enabled { get; }

        // Whether this reactor should be applied on level kind
        public bool IsLevel { get; }

        void Present(Zone approachable);
        void Absent(Zone approachable);
    }
}
