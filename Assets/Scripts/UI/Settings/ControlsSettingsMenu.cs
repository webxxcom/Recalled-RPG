using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlsSettingsMenu : SettingsSection
{
    [SerializeField] InputActionAsset _currentInput;
    [SerializeField] ControlBindItem _inputBindUIItem;
    [SerializeField] GameObject _uiRoot;
    [SerializeField] Button _saveButton;

    InputActionMap _bindMap;
    readonly List<ControlBindItem> _controlBinds = new();
    const string controlsSaveFile = "Controls/inputBinds.json";

    protected override void Awake()
    {
        base.Awake();

        _bindMap = _currentInput.FindActionMap("Player");
        _currentInput.LoadBindingOverridesFromJson(File.ReadAllText(controlsSaveFile));
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _saveButton.onClick.AddListener(OnSaveButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _saveButton.onClick.RemoveListener(OnSaveButtonClick);
    }

    private void Start()
        => InitControls();

    void InitControls()
    {
        foreach (InputAction action in _bindMap)
        {
            ControlBindItem cbi = Instantiate(_inputBindUIItem, _uiRoot.transform);
            cbi.Init(action, this);

            cbi.OnBindUpdate += UpdateControls;
            _controlBinds.Add(cbi);
        }
    }

    void UpdateControls()
    {
        _controlBinds.ForEach(cb => cb.UpdateDisplay());
    }

    InputActionRebindingExtensions.RebindingOperation _currentRebinding;
    public void NotifyRebinding(InputActionRebindingExtensions.RebindingOperation oper)
        => _currentRebinding = oper;

    public override void Close()
    {
        if (_currentRebinding != null && _currentRebinding.started)
            _currentRebinding.Cancel();

        _currentRebinding = null;
    }

    void OnSaveButtonClick()
    {
        string directoryName = Path.GetDirectoryName(controlsSaveFile);
        if (!Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryName);

        File.WriteAllTextAsync(controlsSaveFile, _currentInput.SaveBindingOverridesAsJson());
    }
}
