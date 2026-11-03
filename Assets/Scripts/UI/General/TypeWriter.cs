using TMPro;
using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(TMP_Text))]
    internal sealed class TypeWriter : MonoBehaviour
    {
        [SerializeField] float _delay = 0.035f;

        TMP_Text _text;
        public bool IsTyping { get; private set; }

        private void Awake() => _text = GetComponent<TMP_Text>();

        public void Play(string text)
        {
            _text.text = text;
            _text.maxVisibleCharacters = 0;
            IsTyping = true;
        }

        public void Stop()
        {
            IsTyping = false;
            _text.maxVisibleCharacters = _text.text.Length;
        }

        float _elapsed = 0;
        private void Update()
        {
            if (!IsTyping)
                return;

            _elapsed += Time.unscaledDeltaTime;
            if (_elapsed < _delay)
                return;

            _elapsed = 0;
            if (_text.maxVisibleCharacters < _text.text.Length)
                _text.maxVisibleCharacters++;
            else Stop();
        }
    }
}
