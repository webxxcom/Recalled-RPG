using UnityEngine;
using Recalled.Core;

namespace Recalled.Gameplay
{
    [CreateAssetMenu(menuName = "Dialogue/Event Channel")]
    public class DialogueEventChannel : GameEventChannel<DialoguePayload>
    {
    }
}
