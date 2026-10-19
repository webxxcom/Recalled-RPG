using UnityEngine;

namespace Recalled.Gameplay
{
    public interface ITargetProvider
    {
        GameObject Target { get; }
        bool HasTarget { get; }
    }
}
