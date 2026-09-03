using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public abstract class DefaultDialogComponent : MonoBehaviour
{
    [field: SerializeField] public Image SpriteImage { get; set; }
    [field: SerializeField] public TextMeshProUGUI MainText { get; private set; }
    [field: SerializeField] public int MaxTextLength { get; private set; }

    public float DelayTime { get; } = 0.035f;
    public AudioSource AudioSource { get; private set; }


    protected virtual void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }
}