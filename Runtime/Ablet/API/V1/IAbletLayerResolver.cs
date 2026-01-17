using System;

namespace Ablet.API.V1
{
    /// <summary>
    /// An Ablet Layer Resolver resolves an Ablet Layer by ID.
    /// </summary>
    public interface IAbletLayerResolver : IAbletDefinition
    {
        bool TryResolve(string id, out Type? defType);
    }
}
