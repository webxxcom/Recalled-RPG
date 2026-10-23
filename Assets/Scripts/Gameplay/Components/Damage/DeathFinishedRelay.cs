using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    internal class DeathFinishedRelay : MonoBehaviour
    {
        [SerializeField] UnityEvent _actions;

        public void OnAnimationEvent(string _)
        {
            _actions.Invoke();
        }
    }
}
