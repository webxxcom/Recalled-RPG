namespace Recalled.Dialogue
{
    public partial class Conversation
    {
        public DialogueModel.Line CurrentLine { get; private set; }

        readonly DialogueModel _data;

        public Conversation(DialogueDefinition definition)
        {
            _data = Parser.Parse(definition);

            CurrentLine = _data.StartLine;
        }

        public void Proceed()
        {
            ProceedTo(CurrentLine.Next);
        }

        void ProceedTo(DialogueModel.Line line)
        {
            CurrentLine = line;
        }

        public void Choose(DialogueModel.Choice choice)
        {
            ProceedTo(choice.next);
        }
    }
}
