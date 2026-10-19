using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    [System.Serializable]
    public class Transition
    {
        [SerializeField] Decision _decision;
        [SerializeField] State _trueState;
        [SerializeField] State _falseState;

        public Decision Decision => _decision;
        public State TrueState => _trueState;
        public State FalseState => _falseState;
    }
}
