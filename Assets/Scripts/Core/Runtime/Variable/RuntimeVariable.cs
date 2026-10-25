using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Recalled.Core
{
    /// <summary>
    /// Non-generic base so one custom Editor can target every RuntimeVariable&lt;T&gt;.
    /// (Targeting an open generic type with [CustomEditor] is unreliable across Unity versions.)
    /// </summary>
    public abstract class RuntimeVariableBase : ScriptableObject
    {
        public const string InitialValueField = "_initialValue";
        public const string RuntimeValueField = "_runtimeValue";

        /// <summary>Re-sends the current value to every subscriber, even though it didn't change.</summary>
        public abstract void ForceNotify();

        /// <summary>How many callbacks are currently subscribed. Debug information for the Inspector.</summary>
        public abstract int SubscriberCount { get; }

#if UNITY_EDITOR
        /// <summary>Editor tooling only: put the runtime value back to the authored one and notify.</summary>
        public abstract void EditorResetToInitial();
#endif
    }

    public abstract class RuntimeVariable<T> : RuntimeVariableBase
    {
        [SerializeField] T _initialValue;
        [SerializeField] T _runtimeValue;

        event Action<T> Changed;

        public T Value
        {
            get => _runtimeValue;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_runtimeValue, value))
                    return;

                _runtimeValue = value;
                Changed?.Invoke(value);
            }
        }

        public override void ForceNotify() => Changed?.Invoke(_runtimeValue);
        public override int SubscriberCount => Changed?.GetInvocationList().Length ?? 0;

        public void Subscribe(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            Changed += action;
            action.Invoke(_runtimeValue);
        }
        public void Unsubscribe(Action<T> action) => Changed -= action;

        void OnEnable()
        {
            ResetToInitial();

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

#if UNITY_EDITOR
        void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

        void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
                ResetToInitial();
        }

        public override void EditorResetToInitial() => Value = _initialValue;
#endif
        void ResetToInitial() => _runtimeValue = _initialValue;
    }
}