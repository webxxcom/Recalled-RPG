using Recalled.Dialogue;
using Recalled.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    [RequireComponent(typeof(Toggleable))]
    [RequireComponent(typeof(ScreenController))]
    public class DialogueRunner : MonoBehaviour
    {
        [SerializeField] SpeakerDialogueController _speaker;
        [SerializeField] DialogueEventChannel _dialogueChannel;
        [SerializeField] ChoiceButton _choiceButtonPrefab;
        [SerializeField] Transform _buttonParent;
        [SerializeField] InputActionReference _skipAction;

        readonly List<ChoiceButton> _createdButtons = new();
        ScreenController _screenContoller;
        Conversation _conversation;

        private void Awake()
        {
            if (_speaker == null || _dialogueChannel == null || _choiceButtonPrefab == null || _buttonParent == null || _skipAction == null)
            {
                Debug.LogError($"{gameObject.name}: missing references");
                enabled = false;
            }

            _screenContoller = GetComponent<ScreenController>();

            _dialogueChannel.AddListener(OnDialogueChannel);
        }

        private void OnDestroy()
        {
            _dialogueChannel.RemoveListener(OnDialogueChannel);
        }

        bool _isDirty;
        Coroutine _dialogueCoroutine;
        void OnDialogueChannel(DialoguePayload payload)
        {
            if (_dialogueCoroutine != null)
                return; // Sorry pal but we have our current dialogue

            _conversation = new(payload.dialogueDefinition);
            _speaker.Init(payload.speaker.Faceset);

            _screenContoller.Activate();
            _isDirty = true;
        }

        void LateUpdate()
        {
            if (_isDirty)
            {
                _isDirty = false;
                _dialogueCoroutine = StartCoroutine(Run());
            }
        }

        IEnumerator Run()
        {
            while (_conversation != null)
            {
                // Wait until finished talking
                yield return _speaker.Speak(_conversation.CurrentLine.text, _skipAction.action);

                // What to do next?
                DialogueData.Line line = _conversation.CurrentLine;
                switch (line.Type)
                {
                    case DialogueData.Line.Types.Choices:
                        yield return UserChoosing(line.choices);
                        break;
                    case DialogueData.Line.Types.End:
                        FinishDialogue();
                        break;
                    case DialogueData.Line.Types.Continue:
                        ContinueToNextLine();
                        break;
                }

                yield return WaitForInputAction();
            }
            yield return WaitForInputAction();
            _screenContoller.Deactivate();
            _dialogueCoroutine = null;
        }

        ConsumableValue<DialogueData.Choice> _choice = default;
        IEnumerator UserChoosing(DialogueData.Choice[] choices)
        {
            foreach (var choice in choices)
            {
                ChoiceButton butt = Instantiate(_choiceButtonPrefab, _buttonParent);

                butt.Init(choice.text, () => _choice.Value = choice);
                _createdButtons.Add(butt);
                Canvas.ForceUpdateCanvases();
            }

            // Wait when user chooses smth
            yield return new WaitUntil(() => _choice.Value != null);

            //Remove buttons then
            foreach (var butt in _createdButtons)
                Destroy(butt.gameObject);
            _createdButtons.Clear();

            // Proceed with conversation
            _conversation.Choose((DialogueData.Choice)_choice.Consume());
        }

        void ContinueToNextLine()
        {
            _conversation.Proceed();
        }

        void FinishDialogue()
        {
            _conversation = null;
        }

        IEnumerator WaitForInputAction()
        {
            yield return new WaitUntil(() => _skipAction.action.WasReleasedThisFrame());
        }
    }
}
