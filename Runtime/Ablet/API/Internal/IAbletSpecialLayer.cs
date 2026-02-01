using Ablet.API.V1;

namespace Ablet.API.Internal
{
    public interface IAbletSpecialLayer : IAbletLayer
    {
        // WARNING: Internal priority API and their values are unstable
        int LayerPriority { get; }
        string IdForPriority { get; }
        int InnerPriority { get; }

        bool IsConcreteLayer { get; }
    }
}
