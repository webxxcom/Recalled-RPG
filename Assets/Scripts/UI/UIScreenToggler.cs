using System.Collections.Generic;
using UnityEngine;

namespace Recalled.UI
{
    public sealed class UIScreenToggler : MonoBehaviour
    {
        [SerializeField] Dictionary<VoidGameEvent, Toggleable> _dictionary;

        private void OnEnable()
        {
            foreach (var pair in _dictionary)
                pair.Key.AddListener(pair.Value.RequestToggle);
        }
        private void OnDisable()
        {
            foreach (var pair in _dictionary)
                pair.Key.RemoveListener(pair.Value.RequestToggle);
        }
    }
}
