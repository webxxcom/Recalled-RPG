using Recalled.Core;
using System.Threading.Tasks.Sources;
using UnityEngine;

namespace Recalled.UI
{
    sealed class QuitConfirmationPopup : MonoBehaviour
    {
        [SerializeField] StateScreen _stateScreen;

        [Header("Raises")]
        [SerializeField] VoidGameEvent _quitCancelled;
        [SerializeField] VoidGameEvent _quitConfirmed;

        [Header("Listens")]
        [SerializeField] VoidGameEvent _quitConfirmationRequest;

        GameStateStack _stack;

        private void Awake()
        {
            _stack = GetComponentInParent<GameStateStack>();

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
