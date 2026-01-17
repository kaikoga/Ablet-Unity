using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Models;
using Ablet.Registries.Base;
using Ablet.Utils.Reflection;

namespace Ablet.Registries
{
    public class LayerResolverRegistry : IdModelRegistryBase<IAbletLayerResolver, AbletLayerResolver>
    {
        public static readonly LayerResolverRegistry Instance = new LayerResolverRegistry();

        LayerResolverRegistry()
        {
            Collect(new ModelCollector<IAbletLayerResolver, AbletLayerResolverAttribute, AbletLayerResolver>(def => new AbletLayerResolver(def)));
        }

        public override IEnumerable<AbletLayerResolver> All() => Unordered();
        
        public bool TryResolve(string id, [MaybeNullWhen(false)] out Type defType)
        {
            foreach (var resolver in Unordered())
            {
                if (resolver.TryResolve(id, out defType))
                {
                    return true;
                }
            }
            defType = null;
            return false;
        }
    }
}
