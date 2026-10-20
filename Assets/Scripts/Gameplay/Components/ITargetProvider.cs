using UnityEngine;

namespace Recalled.Gameplay
{
    interface ITargetProvider
    {
        GameObject Target { get; }
        bool HasTarget { get; }
    }
}
