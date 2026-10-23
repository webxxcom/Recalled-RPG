using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    [RequireComponent(typeof(GameStateScreen))]
    public class ScreenOnActionBinder : MonoBehaviour
    {
        [SerializeField] InputActionReference _actionReference;

        GameStateScreen _gameScreen;
        private void Awake()
        {
            _gameScreen = GetComponent<GameStateScreen>();

            _actionReference.action.canceled += OnCancelled;
        }

        private void OnDestroy()
        {
            _actionReference.action.canceled -= OnCancelled;
        }

        void OnCancelled(InputAction.CallbackContext cc)
        {
            if (_gameScreen.IsActive) _gameScreen.Deactivate();
            else _gameScreen.Activate();
        }
    }
}
