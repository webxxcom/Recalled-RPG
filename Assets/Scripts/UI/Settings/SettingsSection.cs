using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class SettingsSection : UIScreen
{
    [SerializeField] Button _button;
    [SerializeField] ISettingsConfigField<Selectable>[] _fields;

    protected override void Awake()
    {
        base.Awake();

        _fields = GetComponentsInChildren<ISettingsConfigField<Selectable>>();
    }

    protected override void Start()
    {
        base.Start();

        _button.onClick.AddListener(Toggle);
    }
}
