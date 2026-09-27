using Newtonsoft.Json;
using Unity.Collections.Tests.CoreCLR.TestJobs;

[System.Serializable]
public struct DialogueData
{
    [System.Serializable]
    public struct Line
    {
        [System.Serializable]
        public class Choice
        {
            public string text;

            public int next;
        }

        public int id;
        public string text;
        public string emotion;

        // A line can: 1. provide choices, 2. have next line, 3. finish dialogue
        public Choice[] choices;
        public int next;
        public int end;

        public Types Type
        {
            get
            {
                if (choices != null) // The player is going to have choices lines
                    return Types.Choices;
                else if (next != 0) // No choices available but also no end then continue
                    return Types.Continue;
                else if (end != 0) // No choices and the end
                    return Types.End;

                throw new JsonReaderException($"Invalid data for a line dialog in {nameof(DialogueData)}");
            }
        }

        public enum Types { Continue, End, Choices }
        public enum Emotions { Surprised, Neutral, Tired }
    }

    public string speaker;
    public Line[] lines;

    public readonly bool TryGetLineWithId(int id, out Line line)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            line = lines[i];
            if (line.id == id)
                return true;
        }

        line = default;
        return false;
    }
}
