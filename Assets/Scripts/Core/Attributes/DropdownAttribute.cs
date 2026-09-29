using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class DropdownAttribute : PropertyAttribute
{
    public readonly string OptionsMemberName;

    public DropdownAttribute(string optionsMemberName)
    {
        OptionsMemberName = optionsMemberName;
    }
}
