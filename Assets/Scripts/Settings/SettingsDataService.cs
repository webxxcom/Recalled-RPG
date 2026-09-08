using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class SettingsDataService
{
    public class Data
    {
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;
        public bool showDamageNumbers;
        public float shakeIntensity;
    } 

    static readonly string FILENAME = "settings.json";
    static readonly string DIRECTORY = Application.persistentDataPath;
    static readonly string PATH = Path.Combine(DIRECTORY, FILENAME);
    static public string[] Fields => typeof(Data).GetFields().Select(f => f.Name).ToArray();

    public Data _data;

    public void SetField(string key, object data)
    {
        FieldInfo field = FindField(key);

        if (!field.FieldType.IsEquivalentTo(data.GetType()))
            throw new ArgumentException($"Settings field '{field}:{field.GetType()}' " +
                $"is not equivalent to provided argument '{data}:{data.GetType()}'");

        field.SetValue(_data, data);
    }

    public object GetFieldValue(string key) => FindField(key).GetValue(_data);

    public static FieldInfo FindField(string key)
    {
        FieldInfo field = typeof(Data).GetField(key)
            ?? throw new ArgumentException($"Tried to set {key} settings field which couldn't be found");

        return field;
    }

    public void Save()
    {
        if (!Directory.Exists(DIRECTORY))
            Directory.CreateDirectory(DIRECTORY);

        File.WriteAllText(PATH, JsonUtility.ToJson(_data));
    }

    public void Load()
    {
        try
        {
            _data = JsonUtility.FromJson<Data>(File.ReadAllText(PATH));
        }
        catch (FileNotFoundException)
        {
            _data = new();
        }
    }
}
