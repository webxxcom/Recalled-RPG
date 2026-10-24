using Recalled.Core;
using System.Threading.Tasks.Sources;
using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(StateScreen))]
    sealed class QuitConfirmationPopup : MonoBehaviour
    {
        [Header("Raises")]
        [SerializeField] VoidGameEvent _quitCancelled;
        [SerializeField] VoidGameEvent _quitConfirmed;

        [Header("Listens")]
        [SerializeField] VoidGameEvent _quitConfirmationRequest;

        StateScreen _stateScreen;

        private void Awake()
        {
            _stateScreen = GetComponent<StateScreen>();

            _quitConfirmationRequest.EventRaised += OnConfirmationRequest;
        }

        private void OnDestroy()
        {
            _quitConfirmationRequest.EventRaised -= OnConfirmationRequest;
        }

        public void OnConfirmationRequest()
        {
            var activated = _stateScreen.Activate();

            // If for some reason the popup didn't appear then cancel by default
            if (!activated)
                Decline();
        }

        public void Confirm()
        {
            _quitConfirmed.Invoke();
        }

        public void Decline()
        {
            _quitCancelled.Invoke();

            _stateScreen.Deactivate();
        }
    }
}
