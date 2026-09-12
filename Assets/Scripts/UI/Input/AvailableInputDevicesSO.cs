using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/Available Input Devices")]
public class AvailableInputDevicesSO : ScriptableObject
{
    [SerializeField] InputDeviceSO[] _devices;

    [Header("Listens to")]
    [SerializeField] StringGameEvent _controlsChanged;

    InputDeviceSO _currentDevice;
    public InputDeviceSO CurrentDevice
    {
        get
        {
            if (_currentDevice == null)
                OnControlsChange("Keyboard");

            return _currentDevice;
        }
        set => _currentDevice = value;
    }

    private void OnEnable()
    {
        _controlsChanged.AddListener(OnControlsChange);
    }

    private void OnDisable()
    {
        _controlsChanged.RemoveListener(OnControlsChange);
    }

    void OnControlsChange(string scheme)
    {
        InputDeviceSO inputDevice = _devices.FirstOrDefault(d => d.Name.ContainsInsensitive(scheme));
        if (inputDevice != null)
            CurrentDevice = inputDevice;
        else
            Debug.Log($"Scheme {scheme} was not found");
    }
}
