using System;

namespace Recalled.Systems.Dialogue
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
