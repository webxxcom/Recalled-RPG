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
        
    protected virtual void OnEnable()
        => _button.onClick.AddListener(OnClick);
    protected virtual void OnDisable()
        => _button.onClick.RemoveListener(OnClick);
    void OnClick()
        => IsActive = !IsActive;
}
