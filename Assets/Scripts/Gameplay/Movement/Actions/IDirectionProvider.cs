using UnityEngine;

namespace Recalled.Gameplay.Movement
{
    public interface IDirectionProvider
    {
        Vector2 Direction { get; }
    }
}
