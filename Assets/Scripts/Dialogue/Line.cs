namespace Recalled.Dialogue
{
    public readonly struct Line
    {
        public readonly string text;
        public readonly EmotionSO emotion;

        internal Line(string text, EmotionSO emotion)
        {
            this.text = text;
            this.emotion = emotion;
        }
    }
}
