public class InvalidLineStateException : DialogueParsingException
{
    public InvalidLineStateException()
    {
    }

    public InvalidLineStateException(string message) : base(message)
    {
    }
}