namespace Recalled.Core
{
    public sealed class GameStateHandle
    {
        public GameStateSO State { get; }

        internal GameStateHandle(GameStateSO state)
        {
            State = state;
        }
    }
}