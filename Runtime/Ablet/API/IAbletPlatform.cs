using System;

namespace Ablet.API
{
    /// <summary>
    /// An Ablet Platform is the definition of Ablet asset with a Unity representation.
    /// </summary>
    public interface IAbletPlatform : IAbletDefinitionBase
    {
        bool ApplyOnPlay { get; }
        Type EntryPointComponentType { get; }
    }
}
