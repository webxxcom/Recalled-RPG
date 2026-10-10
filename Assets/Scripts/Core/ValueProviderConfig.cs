using UnityEngine;

[CreateAssetMenu(menuName = "Value Provider/Config")]
public class ValueProviderConfig : ScriptableObject
{
    [SerializeField] int _maximumValue;
    [SerializeField] int _initValue;
    [SerializeField] bool _isInfinite;

    public int MaximumValue => _maximumValue;
    public int InitValue => _initValue;
    public bool IsInfinite => _isInfinite;
}
