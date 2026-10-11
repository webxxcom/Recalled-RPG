using Recalled.Systems.Dialogue;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    [SelectionBase]
    [RequireComponent(typeof(Interactable))]
    public class Npc : MonoBehaviour
    {
        [SerializeField] DialogueSource _data;

        [Header("Edge event kind, fired each interaction")]
        [SerializeField] UnityEvent _dialogueStarted;

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
            _interactable.RequestInteraction += Interact;
        }

        private void OnDisable()
        {
            _interactable.RequestInteraction -= Interact;
        }

        public void Interact(InteractionProcessor _)
        {
            _dialogueStarted.Invoke();
            _dialogueEventChannel.Invoke(_data);
        }
    }
}
