using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(Toggleable))]
    [RequireComponent(typeof(Canvas))]
    public sealed class GameStateScreen : MonoBehaviour
    {
        [SerializeField] GameStateSO _state;

        public bool IsActive => _handle != null;

        GameStateHandle _handle;
        ToggleableGroup _group;
        Toggleable _toggleable;
        GameStateStack _stack;
        Canvas _canvas;

        void Awake()
        {
            _stack = GetComponentInParent<GameStateStack>();
            _group = GetComponentInParent<ToggleableGroup>();
            _toggleable = GetComponent<Toggleable>();
            _canvas = GetComponent<Canvas>();

            _canvas.enabled = true;
        }

        public void Activate()
        {
            _handle = _stack.Push(_state);

            if (_handle == null)
                return;

            _group.TryAddActive(_toggleable);
        }
        public void Deactivate()
        {
            _stack.Release(_handle);
            _handle = null;

            _group.TryRemoveActive(_toggleable);
        }
    }
}