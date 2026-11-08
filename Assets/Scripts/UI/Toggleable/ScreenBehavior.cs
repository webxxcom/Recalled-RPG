using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI
{
    [RequireComponent(typeof(Toggleable))]
    [RequireComponent(typeof(Canvas))]
    internal sealed class ScreenBehavior : MonoBehaviour, IScreenFocusProvider
    {
        [SerializeField] GameObject _firstSelectedProvider;

        Toggleable _toggleable;
        Canvas _canvas;
        IScreenFocusProvider _provider;
        public event Action FocusTargetChanged;

        private void Awake()
        {
            _toggleable = GetComponent<Toggleable>();
            _canvas = GetComponent<Canvas>();
            _provider = _firstSelectedProvider.GetComponent<IScreenFocusProvider>();

            _toggleable.StateChanged.AddListener(OnToggled);
        }
        private void Start() => OnToggled(_toggleable.IsActive);
        void OnDestroy() => _toggleable.StateChanged.RemoveListener(OnToggled);
        void OnEnable() { if (_provider != null) _provider.FocusTargetChanged += RaiseChanged; }
        void OnDisable() { if (_provider != null) _provider.FocusTargetChanged -= RaiseChanged; }
        void RaiseChanged() => FocusTargetChanged?.Invoke();

        void OnToggled(bool isActived)
        {
            _canvas.enabled = isActived;
        }

        public bool TryGetFocusTarget(out Selectable target)
        {
            if (_provider != null) return _provider.TryGetFocusTarget(out target);
            target = _firstSelectedProvider.GetComponent<Selectable>();
            return target != null;
        }

        public void OnFocused(Selectable selected) => _provider?.OnFocused(selected);

#if UNITY_EDITOR
        void OnValidate()
        {
            if (_firstSelectedProvider != null
                && _firstSelectedProvider.GetComponent<IScreenFocusProvider>() == null
                && _firstSelectedProvider.GetComponent<Selectable>() == null)
                Debug.LogWarning($"{name}: entry focus has neither a Selectable nor an IScreenFocusProvider.", this);
        }
#endif
    }
}
