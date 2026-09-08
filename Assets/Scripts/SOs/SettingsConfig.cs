using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Config")]
public class SettingsConfig : ScriptableObject
{
    SettingsDataService _settings;
    SettingsDataService _copy;

    public SettingsDataService SettingsData => _settings;

    private void OnEnable()
    {
        _settings = new();
        _copy = _settings;
    }

    public void Load()
    {
        _settings.Load();

        _copy = _settings;
    }

    public void Save()
    {
        _settings.Save();
    }
}
