using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [System.Serializable]
    class SettingsSection
    {
        public Button Button;
        public Canvas Section;

        public void OnClick() => Section.enabled = !Section.enabled;
    }

    [SerializeField] SettingsSection _audioSection;
    [SerializeField] SettingsSection _displaySection;
    [SerializeField] SettingsSection _gameplaySection;
    [SerializeField] SettingsSection _controlsSection;

    private void OnEnable()
    {
        _audioSection.Button.onClick.AddListener(_audioSection.OnClick);
        _displaySection.Button.onClick.AddListener(_displaySection.OnClick);
        //_gameplaySection.Button.onClick.AddListener(_gameplaySection.OnClick);
        _controlsSection.Button.onClick.AddListener(_controlsSection.OnClick);
    }
}
