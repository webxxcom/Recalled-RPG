using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerDialogueController : MonoBehaviour
{
    [SerializeField] Transform _buttonGrid;
    [SerializeField] DialogueChoiceButton _choiceButtonPrefab;
    [SerializeField] float _delayTime;

    public IReadOnlyList<DialogueChoiceButton> ChoiceButtons => _playerButtons;

    readonly List<DialogueChoiceButton> _playerButtons = new();
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PutChoices(DialogueData.Line.Choice[] choices, ConsumableValue<DialogueData.Line.Choice> _buttonPressedData)
    {
        int i = 0;
        foreach (var choice in choices)
        {
            DialogueChoiceButton button = Instantiate(_choiceButtonPrefab, Vector2.zero, 
                Quaternion.identity, _buttonGrid.transform);
            
            button.Init($"{i + 1}. {choice.text}", () => { _buttonPressedData.Value = choice; DestroyButtons(); });
            _playerButtons.Add(button);
            ++i;
        }
    }

    public void DestroyButtons()
    {
        foreach (var button in _playerButtons)
            Destroy(button.gameObject);

        _playerButtons.Clear();
    }
}
