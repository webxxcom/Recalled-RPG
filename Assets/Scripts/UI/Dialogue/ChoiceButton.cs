using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
internal class ChoiceButton : MonoBehaviour
{
    [SerializeField] TMP_Text _tmpText;

    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Init(string text, UnityAction action)
    {
        _tmpText.text = text;

        // No unsubscribing is safe because button and the component have the same lifespan
        _button.onClick.AddListener(action);
    }
}
