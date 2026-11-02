namespace Recalled.Gameplay
{
    internal interface IInteractionCondition
    {
        bool Check(InteractionProcessor interaction);
        bool Apply(InteractionProcessor interaction);
    }
}
