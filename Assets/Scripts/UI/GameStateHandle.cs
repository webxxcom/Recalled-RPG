/// <summary>
/// Identity token for one frame on the GameStateStack.
///
/// Why it exists: Stack&lt;T&gt;.Pop() is positional. If Dialogue is pushed, then
/// Pause, and Dialogue ends first, a blind Pop() removes Pause. A handle lets
/// the caller release its own frame by identity, wherever it sits.
///
/// Reference equality is intended: two frames of the same state must stay
/// distinguishable, so Equals/GetHashCode are deliberately not overridden.
/// </summary>
public sealed class GameStateHandle
{
    public GameStateSO State { get; }

    internal GameStateHandle(GameStateSO state)
    {
        State = state;
    }
}