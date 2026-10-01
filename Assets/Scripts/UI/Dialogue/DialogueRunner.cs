using Recalled.Dialogue;
using Recalled.Gameplay;
using Recalled.UI.Dialogue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] EmotionRegistry _emotions;
        [SerializeField] SpeakerRegistry _speakers;
        [SerializeField] CanvasBobbleEffect _continueButton;

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
            _speaker.Init(_speakers.SpriteMap[payload.speaker]);
            _conversation.LineStarted += OnLineStarted;

            _screenContoller.Activate();
            _isDirty = true;
        }

        void LateUpdate()
        {
            if (_isDirty)
            {
                _isDirty = false;
                _conversation.Begin();
            }
        }

        IEnumerator Run(LineStartedPayload payload)
        {
            // Wait until finished talking
            yield return _speaker.Speak(payload.Line.Text, _skipAction.action,
                _emotions.SpriteMap[payload.Line.Emotion]);

            // Decide what to do depending on the payload
            if (payload.Type == Line.Types.End)
                yield return FinishDialogue();
            else if (payload.Type == Line.Types.Choices)
                yield return UserChoosing(payload.Choices);
            else if (payload.Type == Line.Types.Continue)
                yield return ContinueToNextLine();
        }

        void OnLineStarted(LineStartedPayload payload)
        {
            StartCoroutine(Run(payload));
        }

        IEnumerator UserChoosing(IReadOnlyList<string> choices)
        {
            int pressedInd = -1;
            for (int i = 0; i < choices.Count; ++i)
            {
                var choice = choices[i];

                ChoiceButton butt = Instantiate(_choiceButtonPrefab, _buttonParent);
                butt.Init(choice, i);
                butt.Button.onClick.AddListener(() => pressedInd = butt.Index);
                _createdButtons.Add(butt);

                Canvas.ForceUpdateCanvases();
            }

            // Wait when user chooses smth
            yield return new WaitUntil(() => pressedInd != -1);

            //Remove buttons then
            foreach (var butt in _createdButtons)
                Destroy(butt.gameObject);
            _createdButtons.Clear();

            // Proceed with conversation
            _conversation.ChooseOption(pressedInd);
        }

        IEnumerator ContinueToNextLine()
        {
            _continueButton.gameObject.SetActive(true);

            yield return WaitForInputAction();
            _conversation.Advance();
            _continueButton.gameObject.SetActive(false);
        }

        IEnumerator FinishDialogue()
        {
            yield return WaitForInputAction();
            _screenContoller.Deactivate();
            _dialogueCoroutine = null;
        }

        IEnumerator WaitForInputAction()
        {
            yield return new WaitUntil(() => _skipAction.action.WasReleasedThisFrame());
        }
    }
}
