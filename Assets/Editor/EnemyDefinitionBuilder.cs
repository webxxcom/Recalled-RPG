using Recalled.Gameplay;
using UnityEditor;
using UnityEngine;

namespace RecalledEditor
{
    [CustomEditor(typeof(EnemyDefinition))]
    public sealed class EnemyDefinitionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            if (GUILayout.Button("Build Prefab"))
            {
                var prefab = EnemyPrefabBuilder.Build((EnemyDefinition)target);
                if (prefab != null)
                    EditorGUIUtility.PingObject(prefab);
            }
        }
    }
}
