using System.Collections.Generic;
using System.Linq;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Models;
using Ablet.Registries.Base;
using Ablet.Utils.Reflection;

namespace Ablet.Registries
{
    class AssetResolverRegistry : ModelRegistryBase<IAbletAssetResolver, AbletAssetResolver>
    {
        public static readonly AssetResolverRegistry Instance = new AssetResolverRegistry();

        AssetResolverRegistry()
        {
            Collect(new ModelCollector<IAbletAssetResolver, AbletAssetResolverAttribute, AbletAssetResolver>(def => new AbletAssetResolver(def)));
        }

        IEnumerable<AbletAssetResolver> Ordered() => Unordered().OrderBy(value => value.Priority);
        public override IEnumerable<AbletAssetResolver> All() => Ordered();
    }
}
