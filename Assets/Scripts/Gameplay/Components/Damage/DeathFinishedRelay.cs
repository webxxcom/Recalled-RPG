using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    public class DeathFinishedRelay : MonoBehaviour
    {
        [SerializeField] UnityEvent _actions;

        public void OnAnimationEvent(string _)
        {
            _actions.Invoke();
        }
    }
}
