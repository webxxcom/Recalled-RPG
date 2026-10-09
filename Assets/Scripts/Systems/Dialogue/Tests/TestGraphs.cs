using Choice = Recalled.Systems.Dialogue.DialogueGraph.Node.Choice;
using Node = Recalled.Systems.Dialogue.DialogueGraph.Node;

namespace Recalled.Systems.Dialogue.Tests
{
    internal static class TestGraphs
    {
        public static DialogueGraph Linear(params string[] texts)
        {
            Node next = Ending(texts[^1]);

            for (int i = texts.Length - 2; i >= 0; i--)
                next = NewNode(texts[i]).LinkNext(next);

            return new DialogueGraph(next);
        }

        public static DialogueGraph WithChoice()
        {
            var question = NewNode("Question").AddChoices(new[]
            {
                new Choice("Yes", Ending("Said yes")),
                new Choice("No", Ending("Said no")),
            });

            return new DialogueGraph(question);
        }

        public static DialogueGraph WithSingleChoice()
        {
            var question = NewNode("Question").AddChoices(new[]
            {
                new Choice("Okay", Ending("Done")),
            });

            return new DialogueGraph(question);
        }

        public static DialogueGraph WithLoop()
        {
            var start = NewNode("Start");

            var again = NewNode("Again?").AddChoices(new[]
            {
                new Choice("Repeat", start),
                new Choice("Leave", Ending("Bye")),
            });

            start.LinkNext(again);

            return new DialogueGraph(start);
        }

        public static Node Ending(string text) => NewNode(text).EndNode();
        public static Node NewNode(string text) => new(NewLine(text));
        static Line NewLine(string text) => new(text, null);
    }
}