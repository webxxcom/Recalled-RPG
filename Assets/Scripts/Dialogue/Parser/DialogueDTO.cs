[System.Serializable]
internal struct DialogueDTO
{
    [System.Serializable]
    public struct Line
    {
        public int id;
        public string text;
        public string emotion;

        public Choice[] choices;
        public int next;
        public int end;

        public readonly Types Type
        {
            get
            {
                if (choices != null)
                    return Types.Choices;
                else if (next != 0)
                    return Types.Continue;
                else if (end != 0)
                    return Types.End;

                throw new InvalidLineStateException($"Node is not properly defined {id}");
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
    public string emotion;
}
