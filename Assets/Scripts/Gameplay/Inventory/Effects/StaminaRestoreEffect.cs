using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [Serializable]
    public sealed class StaminaRestoreEffect : IEffect
    {
        [SerializeField] int _restorePower;

        public string Description => $"Restores {_restorePower} stamina points";

        public void Apply(GameObject game)
        {
            if (!game.TryGetComponent<Sprint>(out var sprint))
                return;

            sprint.Restore(_restorePower);
        }

        public bool Check(GameObject game)
        {
            if (!game.TryGetComponent<Sprint>(out var sprint))
                return false;

            return !sprint.IsFull;
        }
    }
}
