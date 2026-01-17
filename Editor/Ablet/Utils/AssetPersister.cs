using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.Utils
{
    public static class AssetPersister
    {
        const string OutputPath = "Assets/AbletOutput";
        const string TempPath = "Assets/AbletOutput/__Temp__";

        public static IEnumerable<string> GetManualAssetPaths()
        {
            return AssetDatabase.FindAssets("t:Prefab", new[] { OutputPath })
                .Select(AssetDatabase.GUIDToAssetPath);
        }

        public static void ClearManualAssets()
        {
            AssetDatabase.DeleteAsset(OutputPath);
        }

        public static void ClearTempAssets()
        {
            AssetDatabase.DeleteAsset(TempPath);
        }

        public static void PersistAssets(GameObject rootObject, bool isTemporary)
        {
            try
            {
                var outputDirectory = isTemporary ? TempPath : OutputPath;
                AbletEditorUtil.CreateFolderRecursive(outputDirectory); // need this for GenerateUniqueAssetPath 
                var rootObjectName = rootObject.name == "" ? "_" : rootObject.name;
                var prefabPath = AssetDatabase.GenerateUniqueAssetPath($"{outputDirectory}/{rootObjectName}.prefab");
                var assetName = Path.GetFileNameWithoutExtension(prefabPath);
                var assetFolderPath = $"{outputDirectory}/{assetName}";
                var assetPath = $"{assetFolderPath}/{assetName}.asset";
                AbletEditorUtil.CreateFolderRecursive(assetFolderPath);

                AssetDatabase.StartAssetEditing();
                var container = ScriptableObject.CreateInstance<GeneratedAssets>();
                AssetDatabase.CreateAsset(container, assetPath);
                foreach (var obj in IterateHierarchyAssetReferences(rootObject)
                             .Where(obj => !EditorUtility.IsPersistent(obj)))
                {
                    AssetDatabase.AddObjectToAsset(obj, container);
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(rootObject, prefabPath, InteractionMode.AutomatedAction);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
            }
        }

        static IEnumerable<Object> IterateHierarchyAssetReferences(GameObject rootObject)
        {
            var objectsToResolve = new DistinctQueue<Object>();
            foreach (var transform in rootObject.GetComponentsInChildren<Transform>(true))
            {
                objectsToResolve.EnqueueDistinct(transform);
            }
            var resolvedObjects = new HashSet<Object>();
            while (objectsToResolve.TryDequeue(out var obj))
            {
                switch (obj)
                {
                    case GameObject gameObject:
                        objectsToResolve.EnqueueDistinct(gameObject.transform);
                        break;
                    case Transform transform:
                        foreach (var component in transform.GetComponents<Component>().Where(c => c))
                        {
                            objectsToResolve.EnqueueDistinct(component);
                        }
                        break;
                    case MonoScript _:
                        break;
                    default:
                    {
                        if (obj && !(obj is Component))
                        {
                            resolvedObjects.Add(obj);
                        }
                        using var serializedObject = new SerializedObject(obj);
                        using var serializedProperty = serializedObject.GetIterator();
                        while (true)
                        {
                            var enterChildren = false; 
                            switch (serializedProperty.propertyType)
                            {
                                case SerializedPropertyType.Generic:
                                    enterChildren = true;
                                    break;
                                case SerializedPropertyType.ObjectReference:
                                case SerializedPropertyType.ExposedReference:
                                    if (serializedProperty.objectReferenceValue is { } objReference)
                                    {
                                        objectsToResolve.EnqueueDistinct(objReference);
                                    }
                                    break;
                            }
                            if (!serializedProperty.Next(enterChildren))
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            return resolvedObjects;
        }
    }
}
