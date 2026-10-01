using System;

namespace Recalled.Dialogue
{
    public class Conversation
    {
        public bool HasStarted { get; private set; }
        readonly DialogueGraph _data;
        DialogueGraph.Node _currentNode;

        public event Action<LineStartedPayload> LineStarted;

        internal Conversation(DialogueGraph graph)
        {
            if (graph is null)
                throw new ArgumentNullException($"{nameof(graph)} can't be null");

            _data = graph;
            _currentNode = _data.StartNode;
        }

        public Conversation(DialogueSource definition)
        {
            _data = Parser.Parse(definition);
            _currentNode = _data.StartNode;
        }

        public void Advance()
        {
            if (!HasStarted)
                throw new InvalidOperationException($"The {nameof(Conversation)} has not started yet");
            if (_currentNode.Type != Line.Types.Continue)
                throw new InvalidOperationException($"Trying to {nameof(Advance)} a non-{nameof(Advance)}able line");

            _currentNode = _currentNode.Next;
            LineStarted?.Invoke(new LineStartedPayload(_currentNode));
        }

        public void ChooseOption(int ind)
        {
            if (!HasStarted)
                throw new InvalidOperationException($"The {nameof(Conversation)} has not started yet");
            if (_currentNode.Type != Line.Types.Choices)
                throw new InvalidOperationException($"Trying to {nameof(ChooseOption)} on a non-{nameof(ChooseOption)}able line");

            _currentNode = _currentNode.Choices[ind].Next;
            LineStarted?.Invoke(new LineStartedPayload(_currentNode));
        }

        public void Begin()
        {
            if (HasStarted)
                throw new InvalidOperationException($"Can't {nameof(Begin)} an already Begun conversation");

            HasStarted = true;
            LineStarted?.Invoke(new LineStartedPayload(_currentNode));
        }
    }
}
