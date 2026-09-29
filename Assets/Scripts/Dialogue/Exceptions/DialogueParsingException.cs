using System;

public class DialogueParsingException : Exception
{
    public DialogueParsingException()
    {
    }

    public DialogueParsingException(string message) : base(message)
    {
    }
}
