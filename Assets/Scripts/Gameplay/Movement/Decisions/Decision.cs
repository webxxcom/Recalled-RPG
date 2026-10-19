using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(MovementStateMachine stateMachine, Movementable movementable);
    }
}
