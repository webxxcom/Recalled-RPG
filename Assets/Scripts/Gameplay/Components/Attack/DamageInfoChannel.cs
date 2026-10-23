using Recalled.Core;
using UnityEngine;

namespace Recalled.Gameplay.Combat
{
    [CreateAssetMenu(menuName = "Runtime/Event Channels/Damage Info")]
    public class DamageInfoGameEvent : GameEventChannel<DamageInfo>
    {
    }
}
