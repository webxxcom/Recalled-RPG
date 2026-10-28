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

        public ItemCategory DomainCategory => ItemCategory.Consumable;

        public bool Consume(GameObject game)
        {
            if (_effects.Length == 0) return true;

            bool consumed = false;
            foreach (var effect in _effects)
            {
                if (effect.Check(game))
                {
                    effect.Apply(game);
                    consumed = true;
                }
            }
            return consumed;
        }
    }
}
