using Ablet.Models;

namespace Ablet.Planning
{
    public readonly struct AbletPass
    {
        public readonly AbletLayer Layer;
        public readonly int Depth;

        public AbletPass(AbletLayer layer, int depth = 0)
        {
            Layer = layer;
            Depth = depth;
        }
    }
}
