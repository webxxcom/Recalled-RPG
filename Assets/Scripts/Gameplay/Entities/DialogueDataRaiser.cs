using Recalled.Dialogue;
using UnityEngine;

namespace Recalled.Gameplay
{
    [SelectionBase]
    public class DialogueDataRaiser : MonoBehaviour, IInteractionReactor, IApproachReactor
    {
        [SerializeField] DialogueSource _data;
        [SerializeField] SpeakerSO _speaker;
        [SerializeField] IdleSpriteAnimator _popupAnimator;

        [Header("Raises")]
        [SerializeField] DialogueEventChannel _dialogueEventChannel;

        void Awake()
        {
            if (_data == null || _speaker == null)
            {
                Debug.LogError($"{gameObject.name}: missing some references");
                enabled = false;
            }
        }

        public void Raise()
        {
            _dialogueEventChannel.Invoke(new(_data, _speaker));
        }

        public void ReactToInteraction(IInteractable _)
        {
            Raise();
        }

        public void ReactToApproach(IApproachable approachable)
        {
        }

        public void ReactToRetreat(IApproachable approachable)
        {
            _popupAnimator.Stop();
        }
    }
}
