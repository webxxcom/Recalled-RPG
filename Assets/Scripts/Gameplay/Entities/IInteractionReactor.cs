namespace Recalled.Gameplay
{
    public interface IInteractionReactor
    {
        public bool enabled { get; }
        public void ReactToInteraction(Interactable interactable);
    }
}
