using Newtonsoft.Json;
using UnityEditor;
using static DialogueData.Line;

[System.Serializable]
public class DialogueData
{
    [System.Serializable]
    public class Line
    {
        [System.Serializable]
        public class Choice
        {
            public string text;

            public int next;
        }

        public int id;
        public string text;

        public Choice[] choices;
        public int next;
        public int end;

        public Types Type
        {
            get
            {
                if (choices != null) // The player is going to have choices lines
                    return Types.Choices;
                else if (choices == null && end == 0 && next != 0) // No choices available but also no end then continue
                    return Types.Continue;
                else if (choices == null && end == 1 && next == 0) // No choices and the end
                    return Types.End;

                throw new JsonReaderException("Invalid data for a line dialog in " + nameof(DialogueData));
            }
        }

        public enum Types { Continue, End, Choices}
    }

    public string speaker;
    public Line[] lines;
}
