using Recalled.Systems.Inventory;
using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [Serializable]
    public sealed class Consumable : IModule
    {
        [SerializeReference, SubclassSelector]
        IEffect[] _effects;

        public void Consume(GameObject game)
        {
            foreach (var effect in _effects)
                if (effect.Check(game))
                    effect.Apply(game);
        }
    }
}
