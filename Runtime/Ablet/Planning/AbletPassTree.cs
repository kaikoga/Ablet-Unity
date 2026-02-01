using System.Collections.Generic;
using System.Linq;
using Ablet.Models;

namespace Ablet.Planning
{
    class AbletPassTree
    {
        public readonly AbletLayer Layer;
        public readonly bool IsContainerPass;
        public readonly List<AbletPassTree> Children = new List<AbletPassTree>();

        public AbletPassTree(AbletLayer layer, bool isContainerPass)
        {
            Layer = layer;
            IsContainerPass = isContainerPass;
        }

        public AbletPass ToPass(int depth) => new AbletPass(Layer, IsContainerPass, depth);

        public AbletPassTree? Lookup(AbletLayer layer) =>
            Layer == layer
                ? this
                : Children
                    .Select(child => child.Lookup(layer))
                    .OfType<AbletPassTree>()
                    .FirstOrDefault();
    }
}
