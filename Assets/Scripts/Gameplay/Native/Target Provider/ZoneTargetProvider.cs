using UnityEngine;

namespace Recalled.Gameplay
{
    public class ZoneTargetProvider : TargetProvider
    {
        [Header("Listens to")]
        [SerializeField] GameobjectGameEvent OnTargetEnteredZone;
        [SerializeField] GameobjectGameEvent OnTargetLeftZone;

        void SetTarget(GameObject gameObject)
        {
            CurrentTarget = gameObject != null
                ? gameObject.GetComponentInChildren<HealthResource>().gameObject
                : null;
        }

        public override TargetProvider Init(TargetProviderSO other)
        {
            ZoneTargetProviderSO zoneTargetProviderSO = other as ZoneTargetProviderSO;

            OnTargetEnteredZone = zoneTargetProviderSO.OnTargetEnteredZone;
            OnTargetLeftZone = zoneTargetProviderSO.OnTargetLeftZone;
            if (OnTargetEnteredZone) OnTargetEnteredZone.AddListener(SetTarget);
            if (OnTargetLeftZone) OnTargetLeftZone.AddListener(SetTarget);

            return this;
        }
    }
}
