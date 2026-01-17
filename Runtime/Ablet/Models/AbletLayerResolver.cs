using System;
using System.Diagnostics.CodeAnalysis;
using Ablet.API.V1;

namespace Ablet.Models
{
    public class AbletLayerResolver : IAbletIdModelBase
    {
        readonly IAbletLayerResolver _def;

        public Type DefType => _def.GetType();

        public string Id => _def.Id;
        public string DisplayName => _def.DisplayName;
        public bool TryResolve(string id, [MaybeNullWhen(false)] out Type defType) => _def.TryResolve(id, out defType);

        public AbletLayerResolver(IAbletLayerResolver def) => _def = def;
    }
}
