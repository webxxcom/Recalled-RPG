using System;

namespace Recalled.Dialogue
{
    public class DialogueParsingException : Exception
    {
        public DialogueParsingException()
        {
        }

        public DialogueParsingException(string message) : base(message)
        {
        }
    }
}
