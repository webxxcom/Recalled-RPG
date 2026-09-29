public class InvalidEmotionException : DialogueParsingException
{
    public InvalidEmotionException()
    {
    }

    public InvalidEmotionException(string message) : base(message)
    {
    }
}
