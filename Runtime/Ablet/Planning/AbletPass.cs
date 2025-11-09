using Ablet.API;

namespace Ablet.Planning
{
    public readonly struct AbletPass
    {
        public readonly IAbletLayer Layer;

        public AbletPass(IAbletLayer layer)
        {
            Layer = layer;
        }
    }
}
