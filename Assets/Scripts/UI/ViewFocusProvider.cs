using System;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI
{
    [RequireComponent(typeof(IUiView))]
    internal sealed class ViewFocusProvider : MonoBehaviour, IScreenFocusProvider
    {
        IUiView _view;
        int _rememberedIndex;
        public event Action FocusTargetChanged;

        private void Awake() => _view = GetComponent<IUiView>();
        void OnEnable() => _view.Changed += RaiseChanged;
        void OnDisable() => _view.Changed -= RaiseChanged;
        void RaiseChanged() => FocusTargetChanged?.Invoke();

        public bool TryGetFocusTarget(out Selectable target)
        {
            target = null;

            if (TryGetUsable(_rememberedIndex, out target)) return true;

            for (int i = 0; i < _view.Elements.Count; i++)
                if (TryGetUsable(i, out target)) return true;

            return false;
        }

        public void OnFocused(Selectable selected)
        {
            for (int i = 0; i < _view.Elements.Count; i++)
            {
                if (_view.Elements[i].gameObject == selected.gameObject)
                {
                    _rememberedIndex = i;
                    return;
                }
            }
        }

        bool TryGetUsable(int index, out Selectable selectable)
        {
            selectable = null;
            if (index < 0 || index >= _view.Elements.Count) return false;

            return _view.Elements[index].TryGetComponent(out selectable)
                && selectable.isActiveAndEnabled
                && selectable.IsInteractable();
        }
    }
}
