using System;
using UnityEngine;

namespace Ablet.API.V1
{
    /// <summary>
    /// An Ablet Platform describes a format of the subject for Ablet builds.
    /// </summary>
    public interface IAbletPlatform : IAbletDefinition
    {
        int Priority { get; }
        Type EntrypointComponentType { get; }
        bool FilterEntrypoint(Component component);
    }
}
