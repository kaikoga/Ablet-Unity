using System;
using System.Diagnostics.CodeAnalysis;
using Ablet.API.V1;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.Models
{
    class AbletAssetResolver : IAbletModelBase
    {
        readonly IAbletAssetResolver _def;

        public Type DefType => _def.GetType();

        public int Priority => _def.Priority;
        public string Source => _def.Source;

        public bool TryGetPath(Object obj, GameObject rootObject, [MaybeNullWhen(false)] out string path) => _def.TryGetPath(obj, rootObject, out path);
        public bool TryResolve(string path, Type type, string name, GameObject rootObject, [MaybeNullWhen(false)] out Object obj) => _def.TryResolve(path, type,  name, rootObject, out obj);

        public AbletAssetResolver(IAbletAssetResolver def) => _def = def;
    }
}
