using Recalled.Core;
using UnityEngine;

namespace Recalled.Gameplay
{
    [CreateAssetMenu(menuName = "Runtime/Event Channels/Damage Info")]
    public class DamageInfoGameEvent : GameEventChannel<DamageInfo>
    {
    }
}
