using System.Linq;
using UnityEngine;

namespace Recalled.Core
{
    /// <summary>
    /// App-level owner of quit policy. Both entry points — the in-game Quit button
    /// (via QuitRequested) and the OS / window close (via Application.wantsToQuit) —
    /// go through the same decision in EvaluateQuit().
    /// Lives on the persistent bootstrap object next to the game stack.
    /// </summary>
    [RequireComponent(typeof(GameStateStack))]
    sealed class QuitHandler : MonoBehaviour
    {
        [Header("Listens")]
        [SerializeField] VoidGameEvent _quitRequested;
        [SerializeField] VoidGameEvent _quitConfirmed;
        [SerializeField] VoidGameEvent _quitCancelled;

        [Header("Raises")]
        [SerializeField] VoidGameEvent _quitConfirmationRequested;

        bool RequiresConfirmation => _stack.Frames.Any(h => h.State.RequiresConfirmation);
        GameStateStack _stack;

        // Set once the player has confirmed, so our own final quit
        // isn't cancelled by our own wantsToQuit handler.
        bool _confirmed;

        // True while the confirm dialog is open, so repeated requests
        // (button spam, window close while dialog is up) don't stack dialogs.
        bool _awaitingConfirmation;


        private void Awake()
        {
            _stack = GetComponent<GameStateStack>();
        }

        void OnEnable()
        {
            _quitRequested.EventRaised += OnQuitRequested;
            _quitConfirmed.EventRaised += OnQuitConfirmed;
            _quitCancelled.EventRaised += OnQuitCancelled;
            Application.wantsToQuit += OnWantsToQuit;
        }

        void OnDisable()
        {
            _quitRequested.EventRaised -= OnQuitRequested;
            _quitConfirmed.EventRaised -= OnQuitConfirmed;
            _quitCancelled.EventRaised -= OnQuitCancelled;
            Application.wantsToQuit -= OnWantsToQuit;
        }

        // Entry point 1: the in-game Quit button.
        void OnQuitRequested()
        {
            if (EvaluateQuit())
                ExitApplication();
        }

        // Entry point 2: the OS / window close. Return false to cancel the quit.
        bool OnWantsToQuit()
        {
#if UNITY_EDITOR
            // Unity ignores this return value when exiting Play mode, so don't
            // raise UI events into a scene that is being torn down.
            return true;
#else
            return EvaluateQuit();
#endif
        }

        // Single decision path. Returns true if quitting may proceed right now;
        // otherwise opens the confirmation (once) and returns false.
        bool EvaluateQuit()
        {
            if (_confirmed || !RequiresConfirmation)
                return true;

            if (!_awaitingConfirmation)
            {
                _awaitingConfirmation = true;
                _quitConfirmationRequested.Invoke();
            }

            return false;
        }

        void OnQuitConfirmed()
        {
            // Ignore stray confirmations that don't answer a dialog we opened.
            if (!_awaitingConfirmation)
                return;

            _awaitingConfirmation = false;
            _confirmed = true;
            ExitApplication();
        }

        void OnQuitCancelled()
        {
            _awaitingConfirmation = false;
        }


        static void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}