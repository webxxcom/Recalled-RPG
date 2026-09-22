using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/Available Input Devices")]
public class AvailableInputDevicesSO : ScriptableObject
{
    [SerializeField] InputDeviceSO[] _devices;
    [Header("Sets"), SerializeField] InputDeviceRuntimeVariable _currentInputDevice;
    [Header("Reads"), SerializeField] StringRuntimeVariable _currentControlScheme;

    public InputDeviceSO FindForScheme(string scheme)
    {
        InputDeviceSO inputDevice = _devices.FirstOrDefault(d => d.SchemeName.ToLower().Contains(scheme.ToLower()));
        if (inputDevice != null)
            return inputDevice;
        
        Debug.LogError($"Scheme {scheme} was not found");
        return _devices[0];
    }
}
