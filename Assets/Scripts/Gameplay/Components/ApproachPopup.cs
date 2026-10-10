using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class ApproachPopup : MonoBehaviour, IApproachReactor
    {
        [SerializeField] GameObject _popup;

        private void OnEnable()
        {
            _popup.SetActive(false);
        }

        private void OnDisable()
        {
            _popup.SetActive(false);
        }

        public void ReactToApproach(Approachable approachable)
        {
            _popup.SetActive(true);
        }

        public void ReactToRetreat(Approachable approachable)
        {
            if (enabled) _popup.SetActive(false);
        }
    }
}
