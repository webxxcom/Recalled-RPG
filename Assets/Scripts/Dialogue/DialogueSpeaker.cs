using UnityEngine;

namespace Recalled.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Speaker")]
    public class DialogueSpeaker : ScriptableObject
    {
        [SerializeField] string _name;
        [SerializeField] Sprite _faceset;

        public string Name => _name;
        public Sprite Faceset => _faceset;
    }
}
