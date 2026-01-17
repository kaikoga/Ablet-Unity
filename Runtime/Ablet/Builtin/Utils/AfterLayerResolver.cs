using System;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Registries;

namespace Ablet.Builtin.Utils
{
    [AbletLayerResolver]
    class AfterLayerResolver : IAbletLayerResolver
    {
        string IAbletDefinition.Id => BuiltinLayerResolverIds.AfterLayerResolver;
        string IAbletDefinition.DisplayName => "AfterLayerResolver";
        bool IAbletLayerResolver.TryResolve(string id, out Type? defType)
        {
            if (id.StartsWith(BuiltinLayerIds.AfterPrefix)
                && LayerRegistry.Instance.TryGetById(id[BuiltinLayerIds.AfterPrefix.Length..], out var layer))
            {
                defType = typeof(AfterLayer<>).MakeGenericType(layer.DefType);
                return true;
            }
            defType = null;
            return false;
        }
    }
}
