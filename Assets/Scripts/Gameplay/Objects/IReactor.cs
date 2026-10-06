namespace Recalled.Gameplay
{
    public interface IReactor
    {
        bool enabled { get; }

        void React();
    }
}
