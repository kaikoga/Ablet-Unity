using Ablet.Models;

namespace Ablet.Planning
{
    public readonly struct AbletPass
    {
        public readonly AbletLayer Layer;
        public readonly bool IsContainerPass;
        public readonly int Depth;

        public AbletPass(AbletLayer layer, bool isContainerPass = false, int depth = 0)
        {
            Layer = layer;
            IsContainerPass = isContainerPass;
            Depth = depth;
        }
    }
}
