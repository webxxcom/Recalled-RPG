using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class SettingsSection : UIScreen
{
    [SerializeField] Button _button;

    protected virtual void OnEnable()
        => _button.onClick.AddListener(OnClick);
    protected virtual void OnDisable()
        => _button.onClick.RemoveListener(OnClick);
    void OnClick()
        => IsActive = !IsActive;
}
