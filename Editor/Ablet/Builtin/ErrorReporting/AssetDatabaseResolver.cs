using System;
using System.Linq;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.ErrorReporting
{
    [AbletAssetResolver]
    class AssetDatabaseResolver : IAbletAssetResolver
    {
        int IAbletAssetResolver.Priority => -10000;
        string IAbletAssetResolver.Source => "asset";

        bool IAbletAssetResolver.TryGetPath(Object obj, GameObject rootObject, out string? path)
        {
            if (AssetDatabase.GetAssetPath(obj) is { } assetPath)
            {
                path = assetPath;
                return true;
            }
            path = null!;
            return false;
        }

        bool IAbletAssetResolver.TryResolve(string path, Type type, string name, GameObject rootObject, out Object? obj)
        {
            obj = AssetDatabase.LoadAllAssetsAtPath(path)
                .FirstOrDefault(asset => asset.GetType() == type && asset.name == name);
            return obj is { };
        }
    }
}
