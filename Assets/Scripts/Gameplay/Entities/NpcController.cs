using Recalled.Dialogue;
using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    public class NpcController : EntityController, IInteractable
    {
        [SerializeField] DialogueDefinition _data;
        [SerializeField] DialogueSpeaker _speaker;

        [Header("Raises")]
        [SerializeField] DialogueEventChannel _dialogueEventChannel;

        protected override void Awake()
        {
            base.Awake();

            if (_data == null || _speaker == null)
            {
                Debug.LogError($"{gameObject.name}: missing some references");
                enabled = false;
            }
        }

        public event Action OnInteract;

        public void Interact()
        {
            _dialogueEventChannel.Invoke(new() { dialogueDefinition = _data, speaker = _speaker });

            OnInteract?.Invoke();
        }
    }
}
