using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Global/Configuration")]
public class Configuration : ScriptableObject
{
    [SerializeField] string _filename;
    [SerializeField] bool _showDamageNumbers;

    const string _directory = "Config";

    public bool ShowDamageNumbers => _showDamageNumbers;

    public void Load()
    {
        var a = _showDamageNumbers.Serialize();

        if (!Directory.Exists(_directory))
            Directory.CreateDirectory(_directory);

        string comb = Path.Combine(_directory, _filename);
        File.WriteAllTextAsync(comb, a.json);
    }
}
