using System;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.ErrorReporting
{
    [AbletAssetResolver]
    class RuntimeGeneratedAssetResolver : IAbletAssetResolver
    {
        int IAbletAssetResolver.Priority => int.MaxValue - 1;
        string IAbletAssetResolver.Source => "runtime";

        bool IAbletAssetResolver.TryGetPath(Object obj, GameObject rootObject, out string? path)
        {
            path = obj.GetInstanceID().ToString();
            return true;
        }

        bool IAbletAssetResolver.TryResolve(string path, Type type, string name, GameObject rootObject, out Object? obj)
        {
            obj = null;
            return false;
        }
    }
}
