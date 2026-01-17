using System;
using UnityEngine;

namespace Ablet.API.V1
{
    /// <summary>
    /// An Ablet Platform is the definition of Ablet asset with a Unity representation.
    /// </summary>
    public interface IAbletPlatform : IAbletDefinition
    {
        int Priority { get; }
        Type EntrypointComponentType { get; }
        bool FilterEntrypoint(Component component);
    }
}
