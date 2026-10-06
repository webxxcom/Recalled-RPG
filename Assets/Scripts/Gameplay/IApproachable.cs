using System;

namespace Recalled.Gameplay
{
    public interface IApproachable
    {
        event Action<IApproachable> Approached;

        void Approaching();
        void Retreating();
    }
}