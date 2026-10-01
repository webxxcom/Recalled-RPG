using UnityEngine;

namespace Recalled.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Speaker")]
    public class SpeakerSO : ScriptableObject
    {
        [SerializeField] string _name;

        public string Name => _name;
    }
}
