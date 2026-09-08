using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DropdownAttribute))]
public sealed class DropdownDrawer : PropertyDrawer
{
    private const BindingFlags MemberFlags =
        BindingFlags.Instance | BindingFlags.Static |
        BindingFlags.Public | BindingFlags.NonPublic;

    private const double CacheLifetime = 0.5;

    private string[] _cached;
    private UnityEngine.Object _cachedTarget;
    private double _cachedAt;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String &&
            property.propertyType != SerializedPropertyType.Integer)
        {
            EditorGUI.LabelField(position, label, new GUIContent("Dropdown: string or int only."));
            return;
        }

        if (property.serializedObject.isEditingMultipleObjects)
        {
            EditorGUI.LabelField(position, label, new GUIContent("Dropdown: no multi-edit."));
            return;
        }

        var memberName = ((DropdownAttribute)attribute).OptionsMemberName;
        var target = property.serializedObject.targetObject;
        var options = GetCachedOptions(target, memberName);

        if (options == null || options.Length == 0)
        {
            EditorGUI.LabelField(position, label, new GUIContent($"Dropdown: '{memberName}' unresolved or empty."));
            return;
        }

        EditorGUI.BeginProperty(position, label, property);

        var contents = options.Select(o => new GUIContent(o)).ToArray();
        var current = property.propertyType == SerializedPropertyType.String
            ? Array.IndexOf(options, property.stringValue)
            : property.intValue;

        EditorGUI.BeginChangeCheck();
        var selected = EditorGUI.Popup(position, label, current, contents);

        if (EditorGUI.EndChangeCheck() && selected >= 0 && selected < options.Length)
        {
            if (property.propertyType == SerializedPropertyType.String)
                property.stringValue = options[selected];
            else
                property.intValue = selected;
        }

        EditorGUI.EndProperty();
    }

    private string[] GetCachedOptions(UnityEngine.Object target, string memberName)
    {
        var now = EditorApplication.timeSinceStartup;

        if (_cachedTarget == target && now - _cachedAt < CacheLifetime)
            return _cached;

        _cached = Resolve(target, memberName);
        _cachedTarget = target;
        _cachedAt = now;
        return _cached;
    }

    private static string[] Resolve(object target, string memberName)
    {
        for (var type = target.GetType(); type != null; type = type.BaseType)
        {
            var method = type.GetMethod(memberName, MemberFlags, null, Type.EmptyTypes, null);
            if (method != null)
                return AsStrings(method.Invoke(method.IsStatic ? null : target, null));

            var property = type.GetProperty(memberName, MemberFlags);
            if (property != null && property.CanRead)
            {
                var getter = property.GetGetMethod(true);
                return AsStrings(getter.Invoke(getter.IsStatic ? null : target, null));
            }

            var field = type.GetField(memberName, MemberFlags);
            if (field != null)
                return AsStrings(field.GetValue(field.IsStatic ? null : target));
        }

        return null;
    }

    private static string[] AsStrings(object value) => value switch
    {
        string[] array => array,
        IEnumerable<string> sequence => sequence.ToArray(),
        _ => null
    };
}