namespace Recalled.Dialogue
{
    public class Conversation
    {
        public DialogueGraph.Node CurrentNode { get; private set; }

        readonly DialogueGraph _data;

        public Conversation(DialogueDefinition definition)
        {
            _data = Parser.Parse(definition);

            CurrentNode = _data.StartLine;
        }

        public void ProceedTo(DialogueGraph.Node line)
        {
            CurrentNode = line;
        }
    }
}
