using Recalled.Core;
using Recalled.Gameplay;
using UnityEngine;

namespace Recalled.UI
{
    [CreateAssetMenu(menuName ="Runtime/Event Channels/Health Variable")]
    public sealed class HealthVariableGameEvent : GameEventChannel<HealthValueVariable>
    { }
}
