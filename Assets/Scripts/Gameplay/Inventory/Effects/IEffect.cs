using UnityEngine;

namespace Recalled.Gameplay
{
    public interface IEffect
    {
        bool Check(GameObject game);
        void Apply(GameObject game);
    }
}
