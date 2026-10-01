using Recalled.Dialogue;
using Recalled.Gameplay;
using Recalled.UI.Dialogue;
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
        [SerializeField] InputActionReference _skipAction;
        [SerializeField] EmotionRegistry _emotions;
        [SerializeField] SpeakerRegistry _speakers;
        [SerializeField] CanvasBobbleEffect _continueButton;
        [SerializeField] PrefabPopulator _playerButtons;

        ScreenController _screenContoller;
        Conversation _conversation;

        private void Awake()
        {
            if (_speaker == null || _dialogueChannel == null || _playerButtons == null || _skipAction == null)
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
            if (_conversation != null)
                return; // Sorry pal but we have our current dialogue

            _conversation = new(payload.DialogueDefinition);
            _speaker.Init(_speakers.SpriteMap[payload.Speaker]);
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

            _dialogueCoroutine = null;
        }

        void OnLineStarted(LineStartedPayload payload)
        {
            if (_dialogueCoroutine != null)
                StopCoroutine(_dialogueCoroutine);

            _dialogueCoroutine = StartCoroutine(Run(payload));
        }

        IEnumerator UserChoosing(IReadOnlyList<string> choices)
        {
            int pressedInd = -1;

            _playerButtons.Populate(choices.Count);
            for (int i = 0; i < choices.Count; ++i)
            {
                var choice = choices[i];

                var butt = _playerButtons.Created[i].GetComponent<ChoiceButton>();
                butt.Init(choice, i);
                butt.Button.onClick.AddListener(() => pressedInd = butt.Index);

                Canvas.ForceUpdateCanvases();
            }

            // Wait when user chooses smth
            yield return new WaitUntil(() => pressedInd != -1);

            //Remove buttons and choose the option
            _playerButtons.DestroyAll();
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
            _conversation = null;
        }

        IEnumerator WaitForInputAction()
        {
            yield return new WaitUntil(() => _skipAction.action.WasReleasedThisFrame());
        }
    }
}
