using UnityEngine;

namespace Recalled.Systems.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Source")]
    public class DialogueSource : ScriptableObject
    {
        [SerializeField] TextAsset _textFile;

        DialogueGraph _graph;
        public DialogueGraph Graph => _graph ??= Parser.Parse(_textFile.text);
    }
}
