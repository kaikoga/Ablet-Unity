using System.Collections.Generic;
using System.Linq;
using Ablet.Utils;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Ablet.Building
{
    [PublicAPI]
    public static class AssetPersister
    {
        internal const string OutputPath = "Assets/AbletOutput";
        internal const string TempPath = "Assets/AbletOutput/__Temp__";

        const string OptOutDecisionKey = "Ablet.FlushManualApply.OptOut";

        public static IEnumerable<string> GetManualAssetPaths()
        {
            return AssetDatabase.FindAssets("t:Prefab", new[] { OutputPath })
                .Select(AssetDatabase.GUIDToAssetPath);
        }

        public static void InteractiveClearManualAssets(bool isUserInitiated)
        {
            if (EditorUtility.GetDialogOptOutDecision(DialogOptOutDecisionType.ForThisSession, OptOutDecisionKey))
            {
                return;
            }
            if (EditorUtility.DisplayDialog(
                    Tr("FlushManualApplyLayer::Title"),
                    isUserInitiated ? Tr("FlushManualApplyLayer::UserInitiatedMessage?") : Tr("FlushManualApplyLayer::Message?"),
                    Tr("FlushManualApplyLayer::Ok"),
                    Tr("FlushManualApplyLayer::Cancel")))
            {
                ClearManualAssets();
            }
            else
            {
                EditorUtility.SetDialogOptOutDecision(DialogOptOutDecisionType.ForThisSession, OptOutDecisionKey, true);
            }
        }

        public static void ClearManualAssets()
        {
            AssetDatabase.DeleteAsset(OutputPath);
        }

        public static void ClearTempAssets()
        {
            AssetDatabase.DeleteAsset(TempPath);
        }

        public static void DelayClearTempAssets()
        {
            EditorApplication.delayCall += ClearTempAssets;
        }

        public static IEnumerable<Object> IterateHierarchyAssetReferences(GameObject rootObject)
        {
            var objectsToResolve = new DistinctQueue<Object>();
            foreach (var transform in rootObject.GetComponentsInChildren<Transform>(true))
            {
                objectsToResolve.EnqueueDistinct(transform);
            }
            return ResolveAssetReferences(objectsToResolve);
        }

        public static IEnumerable<Object> IterateAssetObjectReferences(Object asset)
        {
            var objectsToResolve = new DistinctQueue<Object>();
            objectsToResolve.EnqueueDistinct(asset);
            return ResolveAssetReferences(objectsToResolve);
        }

        static IEnumerable<Object> ResolveAssetReferences(DistinctQueue<Object> objectsToResolve)
        {

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
