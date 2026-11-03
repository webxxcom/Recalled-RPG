using UnityEngine;

namespace Recalled.Gameplay
{
    public interface IEffect
    {
        string Description { get; }
        bool Check(GameObject game);
        void Apply(GameObject game);
    }
}
