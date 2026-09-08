using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Config")]
public class SettingsConfig : ScriptableObject
{
    SettingsDataService _settings;

    public SettingsDataService SettingsData => _settings;

    public event Action OnSettingsLoaded;

    private void OnEnable()
    {
        _settings = new();
    }

    public void Load()
    {
        _settings.Load();

        OnSettingsLoaded?.Invoke();
    }

    public void Save()
    {
        _settings.Save();
    }

    public void Revert() => Load();
}
