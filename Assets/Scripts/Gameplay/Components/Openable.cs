using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    public class Openable : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        List<ICondition> _conditions;
        [SerializeField] UnityEvent _opened;
        [SerializeField] Animator _animator;

        private void Start()
        {
            TryOpen();
        }

        public void TryOpen()
        {
            foreach (var condition in _conditions)
            {
                if (!condition.Check())
                    return;
            }

            _opened.Invoke();
            _animator.SetBool(AnimatorParameters.IsOpenHash, true);
        }
    }
}
