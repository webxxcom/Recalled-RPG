using Recalled.Dialogue;

namespace Recalled.Gameplay
{
    public readonly struct DialoguePayload
    {
        public readonly DialogueSource DialogueDefinition;
        public readonly SpeakerSO Speaker;

        public DialoguePayload(DialogueSource dialogueDefinition, SpeakerSO speaker)
        {
            DialogueDefinition = dialogueDefinition;
            Speaker = speaker;
        }
    }
}
