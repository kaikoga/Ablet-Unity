using System;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Registries;

namespace Ablet.Builtin.Utils
{
    [AbletLayerResolver]
    class BeforeLayerResolver : IAbletLayerResolver
    {
        string IAbletDefinition.Id => BuiltinLayerResolverIds.BeforeLayerResolver;
        string IAbletDefinition.DisplayName => "BeforeLayerResolver";
        bool IAbletLayerResolver.TryResolve(string id, out Type? defType)
        {
            if (id.StartsWith(BuiltinLayerIds.BeforePrefix)
                && LayerRegistry.Instance.TryGetById(id[BuiltinLayerIds.BeforePrefix.Length..], out var layer))
            {
                defType = typeof(BeforeLayer<>).MakeGenericType(layer.DefType);
                return true;
            }
            defType = null;
            return false;
        }
    }
}