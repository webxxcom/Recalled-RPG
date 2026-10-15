using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    public class BoolVariableListener : MonoBehaviour
    {
        [SerializeField] BoolRuntimeVariable _variable;
        [SerializeField] UnityEvent<bool> _changed;

        private void OnEnable()
        {
            _variable.Subscribe(OnValueChanged);
        }
        private void OnDisable()
        {
            _variable.Unsubscribe(OnValueChanged);
        }

        void OnValueChanged(bool val)
        {
            _changed.Invoke(val);
        }
    }
}
