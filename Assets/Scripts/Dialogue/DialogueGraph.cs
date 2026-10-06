using System.Collections.Generic;

namespace Recalled.Dialogue
{
    internal partial class DialogueGraph
    {
        public SpeakerSO Speaker { get; private set; }
        public EmotionSO StartingEmotion { get; private set; }
        public Node StartNode { get; private set; }
        public IReadOnlyList<Node> Nodes { get; private set; }

        public DialogueGraph(SpeakerSO speaker, EmotionSO startingEmotion, Node startNode, IReadOnlyList<Node> nodes)
        {
            Speaker = speaker;
            StartingEmotion = startingEmotion;
            StartNode = startNode;
            Nodes = nodes;
        }

        // Constructor for tests because they don't check for speaker etc.
        public DialogueGraph(Node startNode)
        {
            StartNode = startNode;
        }
    }
}
