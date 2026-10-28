using Recalled.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.UI
{
    public class GameChannelListener : MonoBehaviour
    {
        [SerializeField] VoidGameEvent _channel;
        [SerializeField] UnityEvent _actions;

        private void OnEnable() => _channel.EventRaised += InvokeOnInvoke;
        private void OnDisable() => _channel.EventRaised -= InvokeOnInvoke;

        void InvokeOnInvoke() => _actions.Invoke();
    }
}
