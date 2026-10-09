using UnityEngine;

namespace Recalled.Systems.Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Speaker")]
    public class SpeakerSO : ScriptableObject
    {
        [SerializeField] string _name;

        public string Name => _name;
    }
}
