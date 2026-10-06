using System.Collections.Generic;

namespace Recalled.Dialogue
{
    public partial class DialogueGraph
    {
        public SpeakerSO Speaker { get; private set; }
        public EmotionSO StartingEmotion { get; private set; }
        internal Node StartNode { get; private set; }
        internal IReadOnlyList<Node> Nodes { get; private set; }

        internal DialogueGraph(SpeakerSO speaker, EmotionSO startingEmotion, Node startNode, IReadOnlyList<Node> nodes)
        {
            Speaker = speaker;
            StartingEmotion = startingEmotion;
            StartNode = startNode;
            Nodes = nodes;
        }

        // Constructor for tests because they don't check for speaker etc.
        internal DialogueGraph(Node startNode)
        {
            StartNode = startNode;
        }
    }
}
