using Recalled.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RecalledEditor
{
    [CustomEditor(typeof(RuntimeVariableBase), editorForChildClasses: true)]
    public sealed class RuntimeVariabzleEditor : UnityEditor.Editor
    {
        PropertyField _runtimeField;
        Label _subscribers;
        VisualElement _playOnly;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            root.Add(new PropertyField(serializedObject.FindProperty(RuntimeVariableBase.InitialValueField), "Initial Value"));

            _runtimeField = new PropertyField(serializedObject.FindProperty(RuntimeVariableBase.RuntimeValueField), "Runtime Value");
            _runtimeField.RegisterValueChangeCallback(_ =>
            {
                // Fires after the serialized value is applied. In play mode that means the field
                // changed under the setter, so push the new value to subscribers ourselves.
                if (EditorApplication.isPlaying)
                    ((RuntimeVariableBase)target).ForceNotify();
            });
            root.Add(_runtimeField);

            _playOnly = new VisualElement { style = { marginTop = 6 } };

            _subscribers = new Label();
            _playOnly.Add(_subscribers);

            var buttons = new VisualElement { style = { flexDirection = FlexDirection.Row, marginTop = 4 } };
            buttons.Add(new Button(() => ((RuntimeVariableBase)target).ForceNotify()) { text = "Force Notify" });
            buttons.Add(new Button(() => ((RuntimeVariableBase)target).EditorResetToInitial()) { text = "Reset To Initial" });
            _playOnly.Add(buttons);

            root.Add(_playOnly);

            // Keep the play-mode section in sync without a domain reload.
            root.RegisterCallback<AttachToPanelEvent>(_ => EditorApplication.playModeStateChanged += OnPlayModeChanged);
            root.RegisterCallback<DetachFromPanelEvent>(_ => EditorApplication.playModeStateChanged -= OnPlayModeChanged);
            root.schedule.Execute(RefreshSubscribers).Every(250);

            ApplyPlayState(EditorApplication.isPlaying);
            return root;
        }

        void OnPlayModeChanged(PlayModeStateChange state) => ApplyPlayState(EditorApplication.isPlaying);

        void ApplyPlayState(bool playing)
        {
            _runtimeField.SetEnabled(playing);
            _playOnly.style.display = playing ? DisplayStyle.Flex : DisplayStyle.None;
        }

        void RefreshSubscribers()
        {
            if (target == null || !EditorApplication.isPlaying)
                return;

            _subscribers.text = $"Subscribers: {((RuntimeVariableBase)target).SubscriberCount}";
        }
    }
}