public class InvalidLineTypeException : DialogueParsingException
{
    public InvalidLineTypeException()
    {
    }

    public InvalidLineTypeException(string message) : base(message)
    {
    }
}
