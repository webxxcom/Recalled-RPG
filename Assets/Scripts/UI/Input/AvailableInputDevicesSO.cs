using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/Available Input Devices")]
public class AvailableInputDevicesSO : ScriptableObject
{
    [SerializeField] InputDeviceSO[] _devices;
    [SerializeField] StringRuntimeVariable _currentControlScheme;

    public InputDeviceSO CurrentDevice => GetDeviceForScheme(_currentControlScheme.Value);

    InputDeviceSO GetDeviceForScheme(string scheme)
    {
        InputDeviceSO inputDevice = _devices.FirstOrDefault(d => d.Name.ToLower().Contains(scheme.ToLower()));
        if (inputDevice != null)
            return inputDevice;
        
        Debug.LogError($"Scheme {scheme} was not found");
        return null;
    }
}
