[CreateAssetMenu(menuName = "Dialogue/SpeakerMap")]
public class SpeakerRegistry : ScriptableObject
{
    [SerializeField] Dictionary<SpeakerSO, Sprite> _emotionToSpriteMap;

    public IReadOnlyDictionary<SpeakerSO, Sprite> SpriteMap => _emotionToSpriteMap;

    private void OnEnable()
    {
        if (_emotionToSpriteMap.Values.Contains(null))
            Debug.LogWarning($"{name} contains null for some key. Please fix it", this);
    }
}