using System.Collections.Generic;

namespace Recalled.Systems.Dialogue.Tests
{
    internal sealed class ConversationRecorder
    {
        public List<LineStartedPayload> Lines { get; } = new();

        public ConversationRecorder(Conversation conversation)
        {
            conversation.LineStarted += Lines.Add;
        }

        public LineStartedPayload LastLine => Lines[^1];
        public string LastText => LastLine.Line.Text;
    }
}