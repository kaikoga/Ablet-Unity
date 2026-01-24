using System;
using System.Collections.Generic;
using System.IO;
using Ablet.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.Building.Ephemeral
{
    public class AssetPersisterState
    {
        readonly Lazy<string> _assetFolderPath;

        string AssetFolderPath => _assetFolderPath.Value;
        string AssetName => Path.GetFileNameWithoutExtension(_assetFolderPath.Value);

        GeneratedAssets? _currentContainer;
        GeneratedAssets CurrentContainer()
        {
            if (_currentContainer != null)
            {
                return _currentContainer;
            }
            _currentContainer = ScriptableObject.CreateInstance<GeneratedAssets>();
            AssetDatabase.CreateAsset(_currentContainer, $"{AssetFolderPath}/{AssetName}.asset");
            return _currentContainer;
        }

        public AssetPersisterState(GameObject entrypointObject, bool isTemporary)
        {
            var outputDirectory = isTemporary ? AssetPersister.TempPath : AssetPersister.OutputPath;
            var entrypointName = entrypointObject.name == "" ? "_" : entrypointObject.name;
            AbletEditorUtil.CreateFolderRecursive(outputDirectory); // need this for GenerateUniqueAssetPath

            _assetFolderPath = new Lazy<string>(() =>
            {
                var assetFolderPath = AssetDatabase.GenerateUniqueAssetPath($"{outputDirectory}/{entrypointName}");
                AbletEditorUtil.CreateFolderRecursive(assetFolderPath);
                return assetFolderPath;
            });
        }

        public void PersistAssets(IEnumerable<Object> generatedAssets)
        {
            try
            {
                AssetDatabase.StartAssetEditing();
                var currentContainer = CurrentContainer();
                foreach (var obj in generatedAssets)
                {
                    AssetDatabase.AddObjectToAsset(obj, currentContainer);
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
            }
        }

        public void SaveAsPrefab(GameObject rootObject)
        {
            PrefabUtility.SaveAsPrefabAssetAndConnect(rootObject, $"{AssetFolderPath}.prefab", InteractionMode.AutomatedAction);
        }
    }
}
