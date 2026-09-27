using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DialogueChoiceButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;

    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Init(string text, UnityAction action)
    {
        _textMeshPro.text = text;
        _button.onClick.AddListener(action);
    }
}
