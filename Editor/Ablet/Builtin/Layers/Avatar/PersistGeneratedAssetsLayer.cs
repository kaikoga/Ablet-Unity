using System.Collections.Generic;
using System.Linq;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Building.Ephemeral;
using Ablet.Builtin.Utils;
using Ablet.Hooks;
using Ablet.Utils;
using UnityEditor;
using UnityEngine;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class PersistGeneratedAssetsLayer : IAbletLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.PersistGeneratedAssets;
        string IAbletDefinition.DisplayName => "Persist Generated Assets";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<NextLayer<ExportingPhase>>();
        }

        AbletProcedure IAbletLayer.ToProcedure(IBuildArgument argument) => new PersistGeneratedAssetsProcedure();
    }

    class PersistGeneratedAssetsProcedure : AbletBuildProcedure
    {
        public override void Process(IBuildContext context)
        {
            if (context.TryGetArtifact<AssetPersisterState>(out var assetPersister))
            {
                var rootObject = context.CurrentRootObject;
                assetPersister.PersistAssets(IterateHierarchyAssetReferences(rootObject)
                    .Where(obj => !EditorUtility.IsPersistent(obj)));
                assetPersister.SaveAsPrefab(rootObject);
                rootObject.AddComponent<AbletManualAppliedTag>();
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
