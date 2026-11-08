using Recalled.Systems.Inventory;
using System;
using System.Linq;
using UnityEngine;

namespace Recalled.Gameplay.Inventory
{
    [Serializable]
    public sealed class Consumable : IModule, IExtraDescription
    {
        [SerializeReference, SubclassSelector]
        IEffect[] _effects;

        public ItemCategory DomainCategory => ItemCategory.Consumable;
        public string[] ExtraDescription => _effects.Select(e => e.Description).ToArray();

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
