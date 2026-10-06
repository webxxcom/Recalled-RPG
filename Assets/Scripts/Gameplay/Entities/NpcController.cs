using Recalled.Dialogue;
using UnityEngine;

namespace Recalled.Gameplay
{
    [SelectionBase]
    public class NpcController : MonoBehaviour, IInteractionReactor, IApproachReactor
    {
        [SerializeField] DialogueSource _data;
        [SerializeField] GameObject _dialogInfoPopup;

        [Header("Raises")]
        [SerializeField] DialogueEventChannel _dialogueEventChannel;

        void Awake()
        {
            if (_data == null)
            {
                Debug.LogError($"{gameObject.name}: missing some references");
                enabled = false;
            }
        }

        private void OnEnable()
        {
            if (_dialogInfoPopup.activeInHierarchy) _dialogInfoPopup.SetActive(false);
        }

        public void Raise()
        {
            _dialogueEventChannel.Invoke(_data);
        }

        public void ReactToInteraction(IInteractable _)
        {
            Raise();
        }

        public void ReactToApproach(IApproachable _)
        {
            _dialogInfoPopup.SetActive(true);
        }

        public void ReactToRetreat(IApproachable _)
        {
            _dialogInfoPopup.SetActive(false);
        }
    }
}
