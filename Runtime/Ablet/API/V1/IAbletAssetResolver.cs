using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.API.V1
{
    public interface IAbletAssetResolver : IAbletDefinable
    {
        int Priority { get; }

        string Source { get; }

        bool TryGetPath(Object obj, GameObject rootObject, out string? path);
        bool TryResolve(string path, Type type, string name, GameObject rootObject, out Object? obj);
    }
}
