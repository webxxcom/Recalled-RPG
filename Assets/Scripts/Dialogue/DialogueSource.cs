using UnityEngine;

namespace Recalled.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Source")]
    public class DialogueSource : ScriptableObject
    {
        [SerializeField] TextAsset _textFile;

        public string TextData => _textFile.text;
    }
}
