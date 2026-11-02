using Recalled.Core;
using Recalled.Systems.Dialogue;
using UnityEngine;

namespace Recalled.Gameplay
{
    [CreateAssetMenu(menuName = "Runtime/Event Channels/Dialogue Source")]
    public sealed class DialogueEventChannel : GameEventChannel<DialogueSource>
    {
    }
}
