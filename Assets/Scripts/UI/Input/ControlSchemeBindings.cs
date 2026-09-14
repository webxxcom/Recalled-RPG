using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public static class ControlSchemeBindings
{
    static string[] GetBindingPath(InputControl inputControl)
    {
        List<string> controls = new();
        GetBindingPathRecursive(inputControl, controls, "");
        return controls.ToArray();
    }
    static void GetBindingPathRecursive(InputControl inputControl, List<string> controls, string parent)
    {
        var children = inputControl.children;

        if (children.Count != 0)
            foreach (var child in children)
                GetBindingPathRecursive(child, controls, parent + inputControl.name + "/");
        else
            controls.Add(parent + inputControl.name);
        Debug.Log(inputControl.path);
    }

    public static string[] ListAllPathsForScheme(string schemeName)
    {
        if (string.IsNullOrWhiteSpace(schemeName))
            return Array.Empty<string>();

        string[] schemes = schemeName.Split("&");

        // Find available devices whose name matches the scheme name
        var inputDevices = InputSystem.devices.Where(d => schemes.Any(s => d.name.Contains(s)));
        if (inputDevices.Count() == 0)
        {
            Debug.Log($"Incorrect Input device name for {nameof(InputDeviceSO)}");
            return Array.Empty<string>();
        }
        return inputDevices.SelectMany(d => d.allControls.SelectMany(GetBindingPath)).ToArray();
    }

    public static string[] ListInputSchemes()
    {
        return InputSystem.actions.controlSchemes.Select(cs => cs.name).ToArray();
    }

    static bool TryGetActionBindingForScheme(
        InputAction action,
        string scheme,
        string compositePartName,
        out InputBinding result)
    {
        if (!ListInputSchemes().Contains(scheme))
            throw new ArgumentException($"There exists no such scheme as {scheme}");

        result = default;
        for (int i = 0; i < action.bindings.Count; i++)
        {
            result = action.bindings[i];
            if (result.groups.Split(InputBinding.Separator).Contains(scheme))
            {
                if (!string.IsNullOrWhiteSpace(compositePartName))
                {
                    if (result.name.Equals(compositePartName))
                        return true;
                    else continue;
                }
                else return true;
            }
        }
        //Debug.Log($"Couldn't find binding for {action.name}"
        //    + (compositePartName != null ? $" and {nameof(compositePartName)}={compositePartName}" : ""));
        return false;
    }

    public static string GetControlPathNoDevice(
        InputAction action,
        string scheme,
        string compositePartName)
    {
        if (!TryGetActionBindingForScheme(action, scheme, compositePartName, out var inputBinding))
            return null;

        InputControlPath.ToHumanReadableString(
            inputBinding.path,
            out var _,
            out var controlPath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        return controlPath;
    }
}
