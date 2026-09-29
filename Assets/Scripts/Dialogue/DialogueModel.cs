using System;
using System.Collections.Generic;

namespace Recalled.Dialogue
{
    public class DialogueModel
    {
        public class Line
        {
            public int Id { get; private set; }
            public string Text { get; private set; }
            public EmotionSO Emotion { get; private set; }
            public Choice[] Choices { get; private set; }
            public Line Next { get; private set; }

            internal Line(int id, string text, EmotionSO emotion)
            {
                Id = id;
                Text = text;
                Emotion = emotion;
            }

            internal Line ContinueLine(Line next)
            {
                Next = next ?? throw new InvalidLineStateException($"Line can't have null Next if it's a continue line");
                Choices = Array.Empty<Choice>();
                return this;
            }
            internal Line ChoiceLine(Choice[] choices)
            {
                if (choices == null || choices.Length == 0)
                    throw new InvalidLineStateException($"{nameof(choices)} can't be null on a {nameof(Line)} or empty");

                Choices = choices;
                Next = null;
                return this;
            }
            internal Line EndLine()
            {
                Choices = Array.Empty<Choice>();
                Next = null;
                return this;
            }

            public Types Type
            {
                get
                {
                    if (Choices.Length != 0) return Types.Choice;
                    if (Next != null) return Types.Continue;
                    else return Types.End;
                }
            }
            public enum Types { Continue, Choice, End }
        }

        public readonly struct Choice
        {
            public readonly string text;
            public readonly Line next;

            public Choice(string text, Line next)
            {
                this.text = text;
                this.next = next;
            }
        }

        public DialogueSpeaker speaker;
        public EmotionSO startingEmotion;
        public Line StartLine;
        public IReadOnlyList<Line> Lines;
    }
}
