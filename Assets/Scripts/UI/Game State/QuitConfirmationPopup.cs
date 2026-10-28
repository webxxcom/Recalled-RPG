using Recalled.Core;
using System.Threading.Tasks.Sources;
using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(Toggleable))]
    sealed class QuitConfirmationPopup : MonoBehaviour
    {
        [Header("Raises")]
        [SerializeField] VoidGameEvent _quitCancelled;
        [SerializeField] VoidGameEvent _quitConfirmed;

        [Header("Listens")]
        [SerializeField] VoidGameEvent _quitConfirmationRequest;

        Toggleable _toggleable;

        private void Awake()
        {
            _toggleable = GetComponent<Toggleable>();

            _quitConfirmationRequest.EventRaised += OnConfirmationRequest;
        }

        private void OnDestroy()
        {
            _quitConfirmationRequest.EventRaised -= OnConfirmationRequest;
        }

        public void OnConfirmationRequest()
        {
            _toggleable.RequestToggle();

            // If for some reason the popup didn't appear then cancel by default
            if (!_toggleable.IsActive)
                Decline();
        }

        public void Confirm()
        {
            _quitConfirmed.Invoke();
        }

        public void Decline()
        {
            _quitCancelled.Invoke();

            _toggleable.RequestToggle();
        }
    }
}
