public class AbsentSpeakerException : DialogueParsingException
{
    public AbsentSpeakerException()
    {
    }

    public AbsentSpeakerException(string message) : base(message)
    {
    }
}
