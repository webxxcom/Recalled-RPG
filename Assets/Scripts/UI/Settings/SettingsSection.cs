using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class SettingsSection : UIScreen
{
    [SerializeField] Button _button;

    protected override void Start()
    {
        base.Start();

        _button.onClick.AddListener(Toggle);
    }
}
