using NUnit.Framework.Constraints;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.UI
{
    [DisallowMultipleComponent]
    public class Toggleable : MonoBehaviour, IToggleable
    {
        [SerializeField] bool _hidesWhenInactive;
        [SerializeField] UnityEvent<bool> _stateChanged;
        [SerializeField] bool _requestOnStart;

        /// <summary>
        /// IsActive is never called withing the class or the inheritors to avoid races for being active.
        /// The manager, or handler, or anything else manages the activation of the gameobject,
        ///     not the gameobject itself.
        /// </summary>
        public bool IsActive { get; private set; }

        ///<summary>
        /// Event describing the desire to be toggled not an actual toggling.
        /// The manager then allows or disallows the call
        ///</summary>
        public event Action<Toggleable> ToggleRequested;

        public UnityEvent<bool> StateChanged => _stateChanged;

        public void RequestToggle() => ToggleRequested?.Invoke(this);

        private void Start()
        {
            if (_requestOnStart) RequestToggle();
        }

        void UpdateState()
        {
            if (_hidesWhenInactive)
                gameObject.SetActive(IsActive);

            _stateChanged.Invoke(IsActive);
        }

        void IToggleable.SetIsActive(bool value)
        {
            if (value == IsActive)
                return;

            IsActive = value;
            UpdateState();
        }
    }
}
