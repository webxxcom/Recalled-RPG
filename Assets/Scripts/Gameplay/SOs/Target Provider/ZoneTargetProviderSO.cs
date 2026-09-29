using UnityEngine;

namespace Recalled.Gameplay
{
    [CreateAssetMenu(menuName = "Behavior/Zone")]
    public class ZoneTargetProviderSO : TargetProviderSO
    {
        [Header("Listens to")]
        public GameobjectGameEvent OnTargetEnteredZone;
        public GameobjectGameEvent OnTargetLeftZone;

        public override TargetProvider CreateInstance()
            => new ZoneTargetProvider().Init(this);
    }
}
