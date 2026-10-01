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
                yield return _speaker.Speak(_conversation.CurrentNode.Line.text, _skipAction.action,
                    _emotions.SpriteMap[_conversation.CurrentNode.Line.emotion]);

                // What to do next?
                DialogueGraph.Node line = _conversation.CurrentNode;
                switch (line.Type)
                {   
                    case DialogueGraph.Node.Types.Choice:
                        yield return UserChoosing(line.Next);
                        break;
                    case DialogueGraph.Node.Types.End:
                        FinishDialogue();
                        break;
                    case DialogueGraph.Node.Types.Continue:
                        ContinueToNextLine();
                        break;
                }

                yield return WaitForInputAction();
                _continueButton.gameObject.SetActive(false);
            }
            yield return WaitForInputAction();
            _screenContoller.Deactivate();
            _dialogueCoroutine = null;
        }

        ConsumableValue<DialogueGraph.Node> _choice = default;
        IEnumerator UserChoosing(DialogueGraph.Node[] choices)
        {
            foreach (var choice in choices)
            {
                ChoiceButton butt = Instantiate(_choiceButtonPrefab, _buttonParent);

                butt.Init(choice.Line.text, () => _choice.Value = choice.Next[0]);
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
            _conversation.ProceedTo(_choice.Consume());
        }

        void ContinueToNextLine()
        {
            _continueButton.gameObject.SetActive(true);
            _conversation.ProceedTo(_conversation.CurrentNode.Next[0]);
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
