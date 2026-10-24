using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TypeWriter : MonoBehaviour
    {
        [SerializeField] float _delay = 0.035f;
        [SerializeField] InputActionReference[] _actions;

        TMP_Text _text;
        bool _cancellationToken;
        bool _isTyping;

        private void Awake() => _text = GetComponent<TMP_Text>();
        private void OnEnable()
        {
            foreach (var actionRef in _actions) actionRef.action.canceled += Skip;
        }
        private void OnDisable()
        {
            foreach (var actionRef in _actions) actionRef.action.canceled -= Skip;
        }

        void Skip(InputAction.CallbackContext context)
        {
            if (_isTyping) _cancellationToken = true;
        }

        public IEnumerator TypeWriteWithSkip(string text)
        {
            _text.text = text;
            _text.maxVisibleCharacters = 0;
            _isTyping = true;

            float elapsed = 0;
            while (_text.maxVisibleCharacters < _text.text.Length)
            {
                _text.maxVisibleCharacters++;
                while (elapsed < _delay)
                {
                    if (_cancellationToken)
                    {
                        _text.maxVisibleCharacters = _text.text.Length;
                        break;
                    }

                    elapsed += Time.unscaledDeltaTime;
                    yield return null;
                }
                elapsed = 0;
            }
            _isTyping = false;
            _cancellationToken = false;
        }
    }
}
