namespace Recalled.Gameplay
{
    public interface IInteractionCondition
    {
        bool Check(InteractionProcessor interaction);
        bool Apply(InteractionProcessor interaction);
    }
}
