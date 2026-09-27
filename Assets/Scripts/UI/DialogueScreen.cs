using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Button _continueButton;
    [SerializeField] SpeakerDialogueController _leftEntity;
    [SerializeField] PlayerDialogueController _player;
    [SerializeField] ScreenController _screenController;
    [SerializeField] InputActionReference _submitAction;

    ConsumableValue<bool> _enterPressed = new();
    ConsumableValue<DialogueData.Line.Choice> _buttonPressedData = new();

    [Header("Reads")]
    [SerializeField] DialogueVariable _currentDialogue;

    void ResetFields()
    {
        _continueButton.gameObject.SetActive(false);
        _buttonPressedData.Value = default;
        _enterPressed.Value = false;
    }

    void Awake()
    {
        _continueButton.onClick.AddListener(() => _enterPressed.Value = true);
        _currentDialogue.ValueChanged += OnCurrentDialogue;

        if (_currentDialogue.Value != null) OnCurrentDialogue(_currentDialogue.Value);
    }

    void OnEnable()
    {
        BeginDialogue(_currentDialogue.Value);

        _submitAction.action.performed += OnSubmitAction;
    }
    private void OnDisable()
    {
        _submitAction.action.performed += OnSubmitAction;
    }

    void OnDestroy()
    {
        _currentDialogue.ValueChanged -= OnCurrentDialogue;
    }

    void OnSubmitAction(InputAction.CallbackContext _)
    {
        _enterPressed.Value = true;
    }

    void OnCurrentDialogue(DialogueSource dialogueData)
    {
        if (dialogueData == null) _screenController.Deactivate();
        else _screenController.Activate();
    }

    public void BeginDialogue(DialogueSource dialogueData)
    {
        if (dialogueData == null) return;

        DialogueContext context = new()
        {
            faceset = dialogueData.EntitySprite,
            data = JsonUtility.FromJson<DialogueData>(dialogueData.TextFile.text)
        };

        ResetFields();
        _leftEntity.Init(context);
        StartCoroutine(ProcessDialogue(context));
    }

    public IEnumerator ProcessDialogue(DialogueContext context)
    {
        var currentLine = context.data.lines[0]; // The first line is always the opening line

        while (true)
        {
            ResetFields();

            // Wait until left entity stops talking or is out of space for letters
            _leftEntity.StartRevealDialogueText(currentLine.text);
            yield return new WaitUntil(() => _enterPressed.Consume() || !_leftEntity.IsSpeaking);
            _leftEntity.FinishText(currentLine.text);

            switch (currentLine.Type)
            {
                case DialogueData.Line.Types.Choices:
                    _player.PutChoices(currentLine.choices, _buttonPressedData);

                    yield return new WaitUntil(() => _buttonPressedData.Value != default);

                    int nextId = _buttonPressedData.Consume().next;
                    if (context.data.TryGetLineWithId(nextId, out var nextLine))
                        currentLine = nextLine;
                    else
                        Debug.LogException(new MissingReferenceException($"The line with id {nextId} is missing"));

                    break;

                case DialogueData.Line.Types.Continue:
                    // Wait for player continue button push;
                    yield return StartCoroutine(WaitDialogueInput());

                    context.data.TryGetLineWithId(currentLine.next, out currentLine);

                    break;

                case DialogueData.Line.Types.End:
                    yield return StartCoroutine(WaitDialogueInput());

                    EndTalking();

                    yield break;
            }
        }
    }

    IEnumerator WaitDialogueInput()
    {
        _continueButton.gameObject.SetActive(true);

        yield return new WaitUntil(() => _enterPressed.Consume());
    }

    void EndTalking()
    {
        _screenController.Deactivate();
        _currentDialogue.Value = null;
    }
}
