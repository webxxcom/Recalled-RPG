using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Chest))]
    public sealed class ChestApproachPopup : MonoBehaviour, IApproachReactor, IInteractionReactor
    {
        [SerializeField] GameObject _popup;
        Chest _chest;

        private void Awake()
            => _chest = GetComponent<Chest>();

        private void OnEnable()
        {
            _popup.SetActive(false);
        }

        public void ReactToApproach(Approachable approachable)
        {
            if (_chest.CanBeInteracted) _popup.SetActive(true);
        }

        public void ReactToRetreat(Approachable approachable)
        {
            if (enabled) _popup.SetActive(false);
        }

        public void ReactToInteraction(Interactable interactable)
        {
            if (_chest.State == Chest.States.Opened)
            {
                Destroy(_popup);
                enabled = false;
            }
        }
    }
}
