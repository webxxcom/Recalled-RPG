using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(MovementStateMachine stateMachine, Movementable movementable);
    }
}
