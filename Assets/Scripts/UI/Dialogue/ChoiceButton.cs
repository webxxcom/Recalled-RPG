using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
internal class ChoiceButton : MonoBehaviour
{
    [SerializeField] TMP_Text _tmpText;

    public int Index { get; private set; }
    public Button Button => _button;

    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Init(string text, int ind)
    {
        _tmpText.text = text;
        Index = ind;
    }
}
