using System;
using System.Collections.Generic;
using System.Linq;

namespace Recalled.Systems.Dialogue
{
    public partial class DialogueGraph
    {
        internal class Node
        {
            public readonly struct Choice
            {
                public readonly string Text;
                public readonly Node Next;

                public Choice(string text, Node next)
                {
                    // Do not validate data because default(Choice) can't be restricted
                    // Validate at consuming only
                    Text = text;
                    Next = next;
                }
            }

            public readonly Line Line;

            public Node Next { get; private set; }
            public IReadOnlyList<Choice> Choices => _choices;

            bool _isInitialized;
            Choice[] _choices;

            public Node(in Line line)
            {
                Line = line;
                _choices = Array.Empty<Choice>();
            }

            public Node AddChoices(Choice[] choices)
            {
                if (choices == null || choices.Length == 0 || choices.Any(c => c.Next == null || c.Text == null))
                    throw new ArgumentException($"{nameof(Choices)} can't be null or empty for {nameof(Line.Types.Choices)}");
                if (_isInitialized)
                    throw new InvalidOperationException("Trying to re-initialize already initalized Node");

                _choices = (Choice[])choices.Clone();
                _isInitialized = true;
                return this;
            }
            public Node LinkNext(Node next)
            {
                if (_isInitialized)
                    throw new InvalidOperationException($"Trying to re-initialize already initalized Node");

                Next = next ?? throw new ArgumentNullException($"{nameof(next)}");
                _isInitialized = true;
                return this;
            }
            public Node EndNode()
            {
                if (_isInitialized)
                    throw new InvalidOperationException("Trying to re-initialize already initalized Node");

                Next = null;
                _isInitialized = true;
                return this;
            }

            public Line.Types Type
            {
                get
                {
                    if (!_isInitialized)
                        throw new InvalidOperationException("Uninitialized node has no type");

                    if (Next != null) return Line.Types.Continue;
                    else if (Next == null && Choices.Count != 0) return Line.Types.Choices;
                    else return Line.Types.End;
                }
            }
        }
    }
}
