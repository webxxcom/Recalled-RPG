using Recalled.Systems.Dialogue;
using UnityEngine;

namespace Recalled.Gameplay
{
    [SelectionBase]
    public class NpcController : MonoBehaviour, IReactor, IApproachReactor
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

        public void React()
        {
            Raise();
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
