using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/Sprite Sequence")]
public class SpriteSequence : ScriptableObject
{
    [System.Serializable]
    public struct Frame
    {
        public float Time;
        public Sprite Sprite;
    }

    // Stored as a GUID string rather than an Object reference on purpose.
    // A serialized Object reference would pull the source .aseprite import into
    // the build and load it at runtime, for a link only the editor ever uses.
    // A GUID survives moves and renames and costs nothing at runtime.
    [SerializeField, HideInInspector] private string _sourceGuid;
    [SerializeField, HideInInspector] private string _clipName;

    [SerializeField] private List<Frame> _frames = new();
    [SerializeField] private float _length;

    [SerializeField] private bool _loop;

    public IReadOnlyList<Frame> Frames => _frames;
    public float Length => _length;
    public bool Loop => _loop;

    public bool IsValid => _frames.Count > 0 && _length > 0f;

#if UNITY_EDITOR
    public string SourceGuid
    {
        get => _sourceGuid;
        set => _sourceGuid = value;
    }

    public string ClipName
    {
        get => _clipName;
        set => _clipName = value;
    }

    public void SetGeneratedData(List<Frame> frames, float length)
    {
        _frames = frames;
        _length = length;
    }
#endif
}