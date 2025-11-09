using System.Collections.Generic;
using Ablet.API;
using Ablet.API.Attributes;

namespace Ablet.Repositories
{
    public class LayerRepository : DefinitionRepositoryBase<IAbletLayer, AbletLayerAttribute>
    {
        public static readonly LayerRepository Instance = new LayerRepository();

        public override IEnumerable<IAbletLayer> All() => Unordered();
    }
}
