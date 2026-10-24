using Recalled.Gameplay;
using Recalled.Systems.Dialogue;
using Recalled.UI.Dialogue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    [RequireComponent(typeof(Toggleable))]
    [RequireComponent(typeof(StateScreen))]
    public class DialogueRunner : MonoBehaviour
    {
        [SerializeField] SpeakerDialogueController _speaker;
        [SerializeField] DialogueEventChannel _dialogueChannel;
        [SerializeField] InputActionReference[] _skipActions;
        [SerializeField] EmotionRegistry _emotions;
        [SerializeField] SpeakerRegistry _speakers;
        [SerializeField] BobbleEffect _continueButton;
        [SerializeField] PrefabPopulator _playerButtons;

        StateScreen _gameScreen;
        Conversation _conversation;

        private void Awake()
        {
            if (_speaker == null || _dialogueChannel == null || _playerButtons == null || _skipActions.Length == 0)
            {
                Debug.LogError($"{gameObject.name}: missing references");
                enabled = false;
            }

            _gameScreen = GetComponent<StateScreen>();

            _continueButton.gameObject.SetActive(false);
            _dialogueChannel.AddListener(OnDialogueChannel);
        }

        private void OnDestroy()
        {
            _dialogueChannel.RemoveListener(OnDialogueChannel);
        }

        bool _isDirty;
        Coroutine _dialogueCoroutine;
        void OnDialogueChannel(DialogueSource dialogueSource)
        {
            if (_conversation != null)
                return; // Sorry pal but we have our current dialogue

            _conversation = new(dialogueSource.Graph);
            _speaker.Init(_speakers.SpriteMap[dialogueSource.Graph.Speaker]);
            _conversation.LineStarted += OnLineStarted;

            _gameScreen.Activate();
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
            yield return _speaker.Speak(payload.Line.Text,
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
            EventSystem.current.SetSelectedGameObject(_playerButtons.Created[0]);

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
            _continueButton.gameObject.SetActive(true);

            yield return WaitForInputAction();

            _gameScreen.Deactivate();
            _conversation = null;
            _continueButton.gameObject.SetActive(false);
        }

        IEnumerator WaitForInputAction()
        {
            yield return null; // skip this frame's press
            yield return new WaitUntil(() => _skipActions.Any(t => t.action.WasReleasedThisFrame()));
            yield return null; // Consume this one
        }
    }
}
