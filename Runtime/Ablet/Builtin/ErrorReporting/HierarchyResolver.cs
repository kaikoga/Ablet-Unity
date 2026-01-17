using System;
using System.Linq;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.ErrorReporting
{
    [AbletAssetResolver]
    class HierarchyResolver : IAbletAssetResolver
    {
        int IAbletAssetResolver.Priority => -10000;
        string IAbletAssetResolver.Source => "scene";

        bool IAbletAssetResolver.TryGetPath(Object obj, GameObject rootObject, out string? path)
        {
            switch (obj)
            {
                case GameObject gameObject:
                    path = AbletRuntimeUtil.RelativePath(rootObject.transform, gameObject.transform);
                    return true;
                case Component component:
                    path = AbletRuntimeUtil.RelativePath(rootObject.transform, component.transform);
                    return true;
                default:
                    path = null;
                    return false;
            }
        }

        bool IAbletAssetResolver.TryResolve(string path, Type type, string name, GameObject rootObject, out Object? obj)
        {
            if (type == typeof(GameObject))
            {
                var transform = AbletRuntimeUtil.FromRelativePath(rootObject.transform, path);
                obj = transform?.gameObject;
                return true;
            }
            if (type.IsAssignableFrom(typeof(Component)))
            {
                var transform = AbletRuntimeUtil.FromRelativePath(rootObject.transform, path);
                obj = transform?.GetComponents<Component>()
                    .FirstOrDefault(c => c?.GetType() == type);
                return true;
            }
            obj = null;
            return false;
        }
    }
}
