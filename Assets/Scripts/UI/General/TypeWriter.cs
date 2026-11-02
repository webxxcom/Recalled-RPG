using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    [RequireComponent(typeof(TMP_Text))]
    internal sealed class TypeWriter : MonoBehaviour
    {
        [SerializeField] float _delay = 0.035f;
        [SerializeField] InputActionReference[] _actions;

        TMP_Text _text;
        bool _skipToken;

        private void Awake() => _text = GetComponent<TMP_Text>();
        private void OnEnable()
        {
            foreach (var actionRef in _actions)
                actionRef.action.performed += Skip;
        }
        private void OnDisable()
        {
            foreach (var actionRef in _actions)
                actionRef.action.performed -= Skip;
        }
        void Skip(InputAction.CallbackContext _) => _skipToken = true;

        public IEnumerator TypeWriteWithSkip(string text)
        {
            _text.text = text;
            _text.maxVisibleCharacters = 0;
            _skipToken = false;

            float elapsed;
            while (_text.maxVisibleCharacters < _text.text.Length)
            {
                elapsed = 0;
                _text.maxVisibleCharacters++;

                while (elapsed < _delay)
                {
                    if (_skipToken)
                    {
                        _text.maxVisibleCharacters = _text.text.Length;
                        break;
                    }

                    elapsed += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
        }
    }
}
