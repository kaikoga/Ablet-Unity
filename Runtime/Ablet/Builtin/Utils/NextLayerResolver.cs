using System;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Registries;

namespace Ablet.Builtin.Utils
{
    [AbletLayerResolver]
    class NextLayerResolver : IAbletLayerResolver
    {
        string IAbletDefinition.Id => BuiltinLayerResolverIds.NextLayerResolver;
        string IAbletDefinition.DisplayName => "NextLayerResolver";
        bool IAbletLayerResolver.TryResolve(string id, out Type? defType)
        {
            if (id.StartsWith(BuiltinLayerIds.NextPrefix)
                && LayerRegistry.Instance.TryGetById(id[BuiltinLayerIds.NextPrefix.Length..], out var layer))
            {
                defType = typeof(NextLayer<>).MakeGenericType(layer.DefType);
                return true;
            }
            defType = null;
            return false;
        }
    }
}
