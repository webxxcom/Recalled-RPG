using UnityEngine;

namespace Recalled.Gameplay
{
    public interface IChaser
    {
        bool HasTarget { get; }
        Vector2 Direction { get; }
    }
}
