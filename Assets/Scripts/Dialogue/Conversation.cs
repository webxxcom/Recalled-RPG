using UnityEngine;

namespace Recalled.Dialogue
{
    public partial class Conversation
    {
        DialogueData _data;
        int _currLineIndex;

        public DialogueData.Line CurrentLine => _data.lines[_currLineIndex];

        public Conversation(DialogueDefinition definition)
        {
            _data = JsonUtility.FromJson<DialogueData>(definition.TextData);
        }

        void ProceedTo(int lineId)
        {
            _data.TryGetLineWithId(lineId, out _currLineIndex);
        }

        public void Proceed()
        {
            if (CurrentLine.Type != DialogueData.Line.Types.Continue)
                return;

            ProceedTo(CurrentLine.next);
        }

        public void Choose(DialogueData.Choice choice)
        {
            if (CurrentLine.Type != DialogueData.Line.Types.Choices)
                return;

            ProceedTo(choice.next);
        }
    }
}
