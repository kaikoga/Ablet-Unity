using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Models;
using Ablet.Registries.Base;
using Ablet.Utils.Reflection;

namespace Ablet.Registries
{
    public class LayerRegistry : IdModelRegistryBase<IAbletLayer, AbletLayer>
    {
        public static readonly LayerRegistry Instance = new LayerRegistry();

        LayerRegistry()
        {
            Collect(new ModelCollector<IAbletLayer, AbletLayerAttribute, AbletLayer>(def => new AbletLayer(def)));
        }

        public override IEnumerable<AbletLayer> All() => Unordered();

        public override bool TryGetById(string id, [MaybeNullWhen(false)] out AbletLayer value)
        {
            if (base.TryGetById(id, out value))
            {
                return true;
            }
            if (LayerResolverRegistry.Instance.TryResolve(id, out var defType))
            {
                value = ByType(defType);
                return true;
            }
            return false;
        }
    }
}
