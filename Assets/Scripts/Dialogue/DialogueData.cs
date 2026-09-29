using Newtonsoft.Json;

[System.Serializable]
public struct DialogueData
{
    [System.Serializable]
    public struct Line
    {
        public int id;
        public string text;
        public string emotion;

        // A line can: 1. provide choices, 2. have next line, 3. finish dialogue
        public Choice[] choices;
        public int next;
        public int end;

        public readonly Types Type
        {
            get
            {
                if (choices != null) // The player is going to have choices lines
                    return Types.Choices;
                else if (next != 0) // No choices available but also no end then continue
                    return Types.Continue;
                else // Fallback to end
                    return Types.End;
            }
        }

        public enum Types { Continue, End, Choices }
    }

    [System.Serializable]
    public struct Choice
    {
        public string text;

        public int next;
    }

    public string speaker;
    public Line[] lines;

    public readonly bool TryGetLineWithId(int id, out int ind)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            ind = i;
            if (lines[ind].id == id)
                return true;
        }
        ind = default;
        return false;
    }
}
