using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public static class ControlSchemeBindings
{
    static string GetPathOnDevice(InputControl control)
        => control.path[(control.device.path.Length + 1)..];

    public static string[] ListAllPathsForScheme(string schemeName)
    {
        if (string.IsNullOrWhiteSpace(schemeName))
            return Array.Empty<string>();

        var scheme = InputSystem.actions.FindControlScheme(schemeName);
        if (scheme == null)
        {
            Debug.LogWarning($"No control scheme named '{schemeName}' in the project-wide actions.");
            return Array.Empty<string>();
        }

        var devices = InputSystem.devices.Where(d => scheme.Value.SupportsDevice(d)).ToArray();
        if (devices.Length == 0)
        {
            Debug.Log($"No connected device for scheme '{schemeName}'. Connect one to populate the list.");
            return Array.Empty<string>();
        }

        return devices
            .SelectMany(d => d.allControls)
            .Select(GetPathOnDevice)
            .Distinct()
            .ToArray();
    }

    public static string[] ListInputSchemes()
    {
        return InputSystem.actions.controlSchemes.Select(cs => cs.name).ToArray();
    }

    static int GetBindingIndexForScheme(InputAction action, string scheme, string compositePartName)
    {
        var mask = InputBinding.MaskByGroup(scheme);
        if (!string.IsNullOrWhiteSpace(compositePartName))
            mask.name = compositePartName;

        return action.GetBindingIndex(mask);
    }

    public static string GetControlPathNoDevice(
        InputAction action,
        string scheme,
        string compositePartName)
    {
        var ind = GetBindingIndexForScheme(action, scheme, compositePartName);
        if (ind < 0)
            return null;

        action.GetBindingDisplayString(ind, out _, out var controlPath);
        return controlPath;
    }
}
