using System;
using JetBrains.Annotations;

namespace Ablet.API.V1.Attributes
{
    /// <summary>
    /// Marker interface to call initializer method before Ablet.
    /// </summary>
    [MeansImplicitUse]
    public class AbletInitializeOnLoadMethodAttribute : Attribute
    {
    }
}
