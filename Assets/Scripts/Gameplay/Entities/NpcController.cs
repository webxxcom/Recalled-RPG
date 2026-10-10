using Recalled.Systems.Dialogue;
using UnityEngine;

namespace Recalled.Gameplay
{
    [SelectionBase]
    [RequireComponent(typeof(Interactable))]
    public class NpcController : MonoBehaviour, IApproachReactor
    {
        [SerializeField] DialogueSource _data;
        [SerializeField] GameObject _dialogInfoPopup;

        [Header("Raises")]
        [SerializeField] DialogueEventChannel _dialogueEventChannel;

        Interactable _interactable;

        void Awake()
        {
            if (_data == null)
            {
                Debug.LogError($"{gameObject.name}: missing some references");
                enabled = false;
            }
            _interactable = GetComponent<Interactable>();
        }

        private void OnEnable()
        {
            if (_dialogInfoPopup.activeInHierarchy) _dialogInfoPopup.SetActive(false);

            _interactable.RequestInteraction += Raise;
        }

        private void OnDisable()
        {
            _interactable.RequestInteraction -= Raise;
        }

        public void Raise(InteractionProcessor _)
        {
            _dialogueEventChannel.Invoke(_data);
        }

        public void ReactToApproach(Approachable _)
        {
            _dialogInfoPopup.SetActive(true);
        }

        public void ReactToRetreat(Approachable _)
        {
            _dialogInfoPopup.SetActive(false);
        }
    }
}
