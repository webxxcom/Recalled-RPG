namespace Recalled.Dialogue
{
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
}
