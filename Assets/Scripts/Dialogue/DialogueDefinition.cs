using UnityEngine;

namespace Recalled.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Definition")]
    public class DialogueDefinition : ScriptableObject
    {
        [SerializeField] TextAsset _textFile;

        public string TextData => _textFile.text;
    }
}
