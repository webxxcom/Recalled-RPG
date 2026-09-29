using UnityEngine;
using Recalled.Core;

namespace Recalled.Gameplay
{
    [CreateAssetMenu(menuName = "Events/Damage Info")]
    public class DamageInfoGameEvent : GameEventChannel<DamageInfo>
    {
    }
}
