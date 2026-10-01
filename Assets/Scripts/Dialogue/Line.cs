namespace Recalled.Dialogue
{
    public readonly struct Line
    {
        public readonly string Text;
        public readonly EmotionSO Emotion;

        internal Line(string text, EmotionSO emotion)
        {
            Text = text;
            Emotion = emotion;
        }

        public enum Types
        {
            Continue,
            End,
            Choices
        }
    }
}
