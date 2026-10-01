using Recalled.Dialogue;
using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [SelectionBase]
    public class NpcController : MonoBehaviour, IInteractable
    {
        [SerializeField] DialogueSource _data;
        [SerializeField] SpeakerSO _speaker;

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

        public event Action OnInteract;

        public void Interact()
        {
            _dialogueEventChannel.Invoke(new(_data, _speaker));

            OnInteract?.Invoke();
        }
    }
}
