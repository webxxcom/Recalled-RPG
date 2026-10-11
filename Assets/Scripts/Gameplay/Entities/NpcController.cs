using Recalled.Systems.Dialogue;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    [SelectionBase]
    [RequireComponent(typeof(Interactable))]
    public class NpcController : MonoBehaviour, IZoneReactor
    {
        [SerializeField] DialogueSource _data;
        [SerializeField] GameObject _dialogInfoPopup;

        [Header("Edge event kind, fired each interaction")]
        [SerializeField] UnityEvent _interacted;

        [Header("Raises")]
        [SerializeField] DialogueEventChannel _dialogueEventChannel;

        public bool IsLevel => true;

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

            _interactable.RequestInteraction += Interact;
        }

        private void OnDisable()
        {
            _interactable.RequestInteraction -= Interact;
        }

        public void Interact(InteractionProcessor _)
        {
            _interacted.Invoke();
            _dialogueEventChannel.Invoke(_data);
        }

        public void Present(Zone _)
        {
            _dialogInfoPopup.SetActive(true);
        }

        public void Absent(Zone _)
        {
            _dialogInfoPopup.SetActive(false);
        }
    }
}
