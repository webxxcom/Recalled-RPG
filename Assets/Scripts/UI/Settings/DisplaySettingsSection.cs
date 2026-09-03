using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class DisplaySettingsSection : SettingsSection
{
    [SerializeField] Toggle _fullscreen;
    [SerializeField] TMP_Dropdown _resolutionsDropDown;
    [SerializeField] Toggle _vsync;

    List<Resolution> _resolutions;

    protected override void Start()
    {
        base.Start();

        _fullscreen.isOn = Screen.fullScreen;

        _resolutions = Screen.resolutions.ToList();
        _resolutionsDropDown.ClearOptions();
        _resolutionsDropDown.AddOptions(_resolutions.Select(r => r.width + "x" + r.height).ToList());
        _resolutionsDropDown.value = _resolutions.IndexOf(Screen.currentResolution);

        _vsync.isOn = QualitySettings.vSyncCount > 0;
    }

    void OnEnable()
    {
        _fullscreen.onValueChanged.AddListener(OnFullScreen);
        _resolutionsDropDown.onValueChanged.AddListener(OnResolution);
        _vsync.onValueChanged.AddListener(OnVSync);
    }
    void OnDisable()
    {
        _fullscreen.onValueChanged.RemoveListener(OnFullScreen);
        _resolutionsDropDown.onValueChanged.RemoveListener(OnResolution);
        _vsync.onValueChanged.RemoveListener(OnVSync);
    }

    void OnFullScreen(bool isOn)
        => Screen.fullScreen = isOn;

    void OnResolution(int val)
    {
        Resolution res = Screen.resolutions[val];

        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, Screen.currentResolution.refreshRateRatio);
    }

    void OnVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
    }
}
