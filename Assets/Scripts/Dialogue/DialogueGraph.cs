using JetBrains.Annotations;
using System.Collections.Generic;

namespace Recalled.Dialogue
{
    public class DialogueGraph
    {
        public class Node
        {
            public Line Line { get; private set; }
            public Node[] Next { get; private set; }

            public Node(in Line line, Node next)
            {
                Line = line;
                AddNext(next);
            }

            public void AddNext(Node[] nodes)
            {
                Next = nodes;
            }
            public void AddNext(Node node)
            {
                Next = new Node[1] { node };
            }
            public void EndNode()
            {
                Next = new Node[0];
            }

            public Types Type
            {
                get
                {
                    if (Next.Length == 0) return Types.End;
                    else if (Next.Length == 1) return Types.Continue;
                    else return Types.Choice;
                }
            }
            public enum Types { Continue, Choice, End }
        }

        public SpeakerSO speaker;
        public EmotionSO startingEmotion;
        public Node StartLine;
        public IReadOnlyList<Node> Lines;
    }
}
