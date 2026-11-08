using Recalled.Core;
using Recalled.Systems.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.UI
{
    internal abstract class UiView<T> : MonoBehaviour, IUiView where T : class, IView
    {
        [SerializeField] protected PrefabPopulator _populator;

        protected abstract RuntimeAnchor<T> Anchor { get; }

        public IReadOnlyList<GameObject> Elements => _populator.Created;
        protected bool _isDirty;

        public event Action Changed;

        void OnEnable()
        {
            Anchor.Provided += OnAnchorProvided;

            if (Anchor.IsSet) OnAnchorProvided(Anchor.Value);
        }
        void OnDisable()
        {
            Anchor.Provided -= OnAnchorProvided;

            if (Anchor.IsSet) Anchor.Value.Changed -= OnViewChanged;
        }

        void OnAnchorProvided(T view)
        {
            view.Changed += OnViewChanged;

            OnViewChanged();
        }
        void OnViewChanged() => _isDirty = true;

        protected abstract void Refresh();

        private void LateUpdate()
        {
            if (_isDirty)
            {
                _isDirty = false;

                Refresh();
                Changed?.Invoke();
            }
        }
    }
}
