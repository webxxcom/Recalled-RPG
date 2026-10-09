using System;
using System.Collections.Generic;
using System.Linq;

namespace Recalled.Systems.Dialogue
{
    public readonly struct LineStartedPayload
    {
        public readonly Line.Types Type;

        public readonly Line Line;
        public readonly IReadOnlyList<string> Choices;

        internal LineStartedPayload(DialogueGraph.Node node)
        {
            Type = node.Type;
            Line = node.Line;

            if (Type == Line.Types.Choices)
                Choices = node.Choices.Select(n => n.Text).ToArray();
            else Choices = Array.Empty<string>();
        }
    }
}
