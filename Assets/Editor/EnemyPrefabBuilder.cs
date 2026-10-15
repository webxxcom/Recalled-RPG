using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Recalled.Gameplay.Editor
{
    /// <summary>
    /// Edit-time composition: for each EnemyDefinition, produce a prefab VARIANT of its base prefab
    /// with the visual prefab nested under VisualRoot and the definition assigned.
    ///
    /// Output: {definition folder}/Generated/{definition name}.prefab
    /// Rebuilding is idempotent: it replaces the visual child and keeps any other overrides
    /// you made to the variant by hand.
    /// </summary>
    public static class EnemyPrefabBuilder
    {
        const string OutputFolder = "Assets/Prefabs/Enemies";
        const string VisualChildName = "Visual";

        [MenuItem("Recalled/Enemies/Build All Enemy Prefabs")]
        public static void BuildAll()
        {
            var guids = AssetDatabase.FindAssets("t:" + nameof(EnemyDefinition));
            int built = 0;

            foreach (var guid in guids)
            {
                var definition = AssetDatabase.LoadAssetAtPath<EnemyDefinition>(AssetDatabase.GUIDToAssetPath(guid));
                if (Build(definition) != null)
                    built++;
            }

            Debug.Log($"EnemyPrefabBuilder: built {built}/{guids.Length} enemy prefabs.");
        }

        public static GameObject Build(EnemyDefinition definition)
        {
            if (!Validate(definition, out string error))
            {
                Debug.LogError($"EnemyPrefabBuilder: {error}", definition);
                return null;
            }

            string path = OutputPath(definition);
            EnsureVariantExists(definition.BasePrefab, path);

            // LoadPrefabContents gives an isolated, editable copy of the prefab — no scene is touched.
            var root = PrefabUtility.LoadPrefabContents(path);
            try
            {
                var visualRoot = root.GetComponentInChildren<VisualRoot>(true);
                ClearChildren(visualRoot.transform);

                // InstantiatePrefab (not Instantiate) keeps the link, so this becomes a NESTED prefab:
                // layer changes in the .ase file still flow through to every built enemy.
                var visual = (GameObject)PrefabUtility.InstantiatePrefab(definition.VisualPrefab, visualRoot.transform);
                visual.name = VisualChildName;
                visual.transform.localPosition = Vector3.zero;
                visual.transform.localRotation = Quaternion.identity;
                visual.transform.localScale = Vector3.one;

                ConfigureAnimator(visual, definition);
                AssignDefinition(root, definition);

                PrefabUtility.SaveAsPrefabAsset(root, path);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }

            var result = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Debug.Log($"EnemyPrefabBuilder: built {path}", result);
            return result;
        }

        // ------------------------------------------------------------------ steps

        static bool Validate(EnemyDefinition definition, out string error)
        {
            error = null;

            if (definition == null) { error = "definition is null"; return false; }
            if (definition.BasePrefab == null) { error = $"{definition.name}: Base Prefab is not set"; return false; }
            if (definition.VisualPrefab == null) { error = $"{definition.name}: Visual Prefab is not set"; return false; }

            var visualRoot = definition.BasePrefab.GetComponentInChildren<VisualRoot>(true);
            if (visualRoot == null)
            {
                error = $"{definition.name}: base prefab has no child with a {nameof(VisualRoot)} component";
                return false;
            }
            if (visualRoot.transform.childCount > 0)
            {
                error = $"{definition.name}: {nameof(VisualRoot)} in the base prefab must have no children (the builder owns them)";
                return false;
            }
            bool visualHasAnimator = definition.VisualPrefab.GetComponentInChildren<Animator>(true) != null;

            return true;
        }

        /// <summary>
        /// Guarantees an Animator on the visual ROOT and assigns the definition's controller.
        /// The Animator must sit on the rig root: Aseprite-generated clips bind renderers by path
        /// relative to the object the Animator is on ("body", "eyes"...). Putting it on the entity
        /// root would change those paths to "VisualRoot/Visual/body" and nothing would animate.
        /// </summary>
        static void ConfigureAnimator(GameObject visual, EnemyDefinition definition)
        {
            var animator = visual.GetComponentInChildren<Animator>(true);

            if (animator == null)
            {
                // Added on the nested instance: recorded as a component override on the variant,
                // so it survives rebuilds and the imported prefab stays untouched.
                animator = visual.AddComponent<Animator>();
                animator.applyRootMotion = false;
                animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
            }
            else if (animator.gameObject != visual)
            {
                Debug.LogWarning(
                    $"EnemyPrefabBuilder: {definition.name}: Animator is on '{animator.name}', not on the visual root. " +
                    "Clip paths are relative to the Animator's object — make sure that's what the clips expect.",
                    definition);
            }
        }

        static string OutputPath(EnemyDefinition definition) => $"{OutputFolder}/{definition.name}.prefab";

        static void EnsureFolderExists(string assetFolder)
        {
            if (AssetDatabase.IsValidFolder(assetFolder))
                return;

            string parent = Path.GetDirectoryName(assetFolder).Replace('\\', '/');
            string name = Path.GetFileName(assetFolder);

            EnsureFolderExists(parent);
            AssetDatabase.CreateFolder(parent, name);
        }

        /// <summary>Creates the variant asset if missing, or recreates it if its base prefab changed.</summary>
        static void EnsureVariantExists(GameObject basePrefab, string path)
        {
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing != null)
            {
                bool sameBase = PrefabUtility.GetCorrespondingObjectFromSource(existing) == basePrefab;
                if (sameBase)
                    return;

                AssetDatabase.DeleteAsset(path); // base changed: a variant can't be re-parented, so start over
            }
            EnsureFolderExists(OutputFolder);

            // A prefab saved from an INSTANCE of another prefab becomes a variant of it.
            // A preview scene hosts that instance so the user's open scene is never modified.
            var scene = EditorSceneManager.NewPreviewScene();
            try
            {
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(basePrefab, scene);
                PrefabUtility.SaveAsPrefabAsset(instance, path);
            }
            finally
            {
                EditorSceneManager.ClosePreviewScene(scene);
            }
        }

        static void ClearChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(parent.GetChild(i).gameObject);
        }

        static void AssignDefinition(GameObject root, EnemyDefinition definition)
        {
            if (!root.TryGetComponent(out EntityIdentity identity))
                identity = root.AddComponent<EntityIdentity>();

            // SerializedObject writes the private field the same way the Inspector would.
            var serialized = new SerializedObject(identity);
            serialized.FindProperty(EntityIdentity.DefinitionFieldName).objectReferenceValue = definition;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}